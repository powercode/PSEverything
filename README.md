# PSEverything
PowerShell cmdlet for the local search engine Everything

_Everything_ is a blazingly fast local search engine by David Carpenter, found at http://www.voidtools.com/

To use it, download Everything and install it as a service.

Install PSEverything from PowerShell Gallery.

```powershell
Install-Module PSEverything
```

Then you can so things like
```powershell
Search-Everything -Extension cpp,h -Global -Filter Bytes | Get-Item
```
That almost instantly finds all cpp and h files with Bytes in it's name on all (-Global) NTFS drives.

On my system,
```powershell
Search-Everything -Extension h -Global
```
returned 61082 '.h' files in 344 ms.
