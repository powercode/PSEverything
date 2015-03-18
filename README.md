# PSEverything
PowerShell commandlets for the local search engine Everything

Everything is a blazingly fast local search engine by David Carpenter, found at http://www.voidtools.com/

To use it, download Everything and install it as a service.

Then you can so things like
Search-Everything -Extension cpp,h -FilePattern Bytes | Get-Item

That almost instantly finds all cpp and h files with Bytes in it's name on all NTFS drives.

On my system, Search-Everything -ext h returned 61082 '.h' files in 344 ms.
