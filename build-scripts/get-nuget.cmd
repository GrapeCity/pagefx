mkdir .nuget
powershell -Command "(new-object System.Net.WebClient).DownloadFile('http://az320820.vo.msecnd.net/downloads/nuget.exe', '.nuget\NuGet.exe')"
