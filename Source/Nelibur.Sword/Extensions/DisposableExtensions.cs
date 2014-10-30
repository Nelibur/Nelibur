using System;

namespace Nelibur.Sword.Extensions
{
    public static class DisposableExtensions
    {
        public static void SafeDispose(this IDisposable disposable, Action<Exception> exceptionHandler = null)
        {
            if (disposable == null)
            {
                return;
            }

            try
            {
                disposable.Dispose();
            }
            catch (Exception exp)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(exp);
                }
            }
        }
    }
}
