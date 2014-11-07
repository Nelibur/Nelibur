param($installPath, $toolsPath, $package, $project)
 
# open splash page on package install
# don't open if it is installed as a dependency
 
try
{
  $url = "http://nelibur.org/"
  $packageName = "nelibur"
  $dte2 = Get-Interface $dte ([EnvDTE80.DTE2])
 
  if ($dte2.ActiveWindow.Caption -eq "Package Manager Console")
  {
    # user is installing from VS NuGet console
    # get reference to the window, the console host and the input history
    # show webpage if "install-package Nelibur" was last input
 
    $consoleWindow = $(Get-VSComponentModel).GetService([NuGetConsole.IPowerConsoleWindow])
 
    $props = $consoleWindow.GetType().GetProperties([System.Reflection.BindingFlags]::Instance -bor `
      [System.Reflection.BindingFlags]::NonPublic)
 
    $prop = $props | ? { $_.Name -eq "ActiveHostInfo" } | select -first 1
    if ($prop -eq $null) { return }
  
    $hostInfo = $prop.GetValue($consoleWindow)
    if ($hostInfo -eq $null) { return }
 
    $history = $hostInfo.WpfConsole.InputHistory.History
 
    $lastCommand = $history | select -last 1
 
    if ($lastCommand)
    {
      $lastCommand = $lastCommand.Trim().ToLower()
      if ($lastCommand.StartsWith("install-package") -and $lastCommand.Contains($packageName))
      {
        $dte2.ItemOperations.Navigate($url) | Out-Null
      }
    }
  }
  else
  {
    # user is installing from VS NuGet dialog
    # get reference to the window, then smart output console provider
    # show webpage if messages in buffered console contains "installing...Nelibur" in last operation
 
    $instanceField = [NuGet.Dialog.PackageManagerWindow].GetField("CurrentInstance", [System.Reflection.BindingFlags]::Static -bor `
      [System.Reflection.BindingFlags]::NonPublic)
    $consoleField = [NuGet.Dialog.PackageManagerWindow].GetField("_smartOutputConsoleProvider", [System.Reflection.BindingFlags]::Instance -bor `
      [System.Reflection.BindingFlags]::NonPublic)
    if ($instanceField -eq $null -or $consoleField -eq $null) { return }
 
    $instance = $instanceField.GetValue($null)
    if ($instance -eq $null) { return }
 
    $consoleProvider = $consoleField.GetValue($instance)
    if ($consoleProvider -eq $null) { return }
 
    $console = $consoleProvider.CreateOutputConsole($false)
 
    $messagesField = $console.GetType().GetField("_messages", [System.Reflection.BindingFlags]::Instance -bor `
      [System.Reflection.BindingFlags]::NonPublic)
    if ($messagesField -eq $null) { return }
 
    $messages = $messagesField.GetValue($console)
    if ($messages -eq $null) { return }
 
    $operations = $messages -split "=============================="
 
    $lastOperation = $operations | select -last 1
 
    if ($lastOperation)
    {
      $lastOperation = $lastOperation.ToLower()
 
      $lines = $lastOperation -split "`r`n"
 
      $installMatch = $lines | ? { $_.StartsWith("------- installing..." + $packageName) } | select -first 1
 
      if ($installMatch)
      {
        $dte2.ItemOperations.Navigate($url) | Out-Null
      }
    }
  }
}
catch
{
  # stop potential errors from bubbling up
  # worst case the splash page won't open
}
