@echo off
echo Building project...
dotnet build
if %ERRORLEVEL% neq 0 (
    echo Build failed! Exiting...
    pause
    exit /b %ERRORLEVEL%
)

echo Running project...
dotnet run

pause