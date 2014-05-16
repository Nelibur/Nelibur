# Nelibur Release Notes

## 3.0.1-beta2

### New Features/Improvements

* Changed client's methods signature  
  New API methods include:
  * `public TResponse Get<TResponse>(object request)`
  * `public TResponse Post<TResponse>(object request)`
  * `public TResponse Put<TResponse>(object request)`
  * `public TResponse Delete<TResponse>(object request)`
  * `etc`

## 3.0.1-beta1

### New Features

* Changed url serialization. Urls are readable
  * `http://localhost/GetWithResponse?type=PresentRequest&Country=Sheldonopolis&Status=Pending`
