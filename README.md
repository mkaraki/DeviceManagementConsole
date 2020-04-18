# Device Management Console
Remote Computer Management System

## Client
`DeviceManagementConsole.WindowsClient` works with Windows 10.

### Config file
Put config file into `C:\dmc.yaml`. 
```yaml
ComputerUnique: foo # PC name
Server: dmc-server.example.com # DMC Server
Port: 2515 # DMC Server Port
Interval: 60000 # Report Interval (ms)
KeepaliveEnabled: true # Use Keepalive?
KeepaliveInterval: 15000 # Keepalive Interval (Recommend less than 30000ms)
TaskEnabled: true # Use Remote Control?
TaskInterval: 5000 # Remote Task Check Interval (Recommend less than 10000ms)
```

## Server
Server works with Apache2 with PHP mod.
Put files in `dmc_server` into `htdocs`.

### Port
We recommend to use port `2515`.

### Notice
This server application will save file directly.
Please be careful for your security.

### For Security
1. Deny access from ouside of LAN.
2. Don't publish to WAN
3. Set authentication to `query.php` and `queuetask.php`.
4. Support for HTTPS. (You have to rewrite `/DeviceManagementConsole.Shared/Messenger.cs`'s `AccessBaseUrl`)
5. Use your VPN to connect Web Console

### For Debug
You can run debug server with `debug.cmd`.(Require php for windows in PATH. No UNC path supported.)

## Web Console
Access server's IP and configurated port.

Example:
```
http://dmc-server.example.com:2515
```