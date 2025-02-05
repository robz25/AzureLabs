# .net9 WebJob to run on Azure App Service

## Creating the webjob

```bat
dotnet new console # creates new console app
dotnet build # buisl project
dotnet run # runs project

#Run this to generate executable with all dlls in publish folder
dotnet publish -c Release -o ./
publish                                    

#Then create a run.cmd batch file, contents:
@echo off
ExecFileName.exe

# put that file inside the publish folder and compress to zip
# then upload to Azure in webJobs view
```

## Local variables for testing

Create local.settings.json in project root
Add any env vars inside
```
{
  "IsEncrypted": false,
  "Values": {
    "APPINSIGHTS_CONNECTIONSTRING": "your-connection-string"
  }
}
```

Refer to them on code

```c#
string appInsightsConnectionString = Environment.GetEnvironmentVariable("APPINSIGHTS_CONNECTIONSTRING");
```

## Query logs in application insights

```kql
traces
| where cloud_RoleName == "MyWebJob6"
| project timestamp, message
```

## Quick test

In this case the ai6wj.zip is already included we just need to upload this file to azure and make sure our app has the setting APPLICATIONINSIGHTS_CONNECTION_STRING