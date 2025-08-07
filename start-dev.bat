@echo off
setlocal enabledelayedexpansion

:: Function to kill processes on specific ports
:kill_port
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :%1') do (
    taskkill /F /PID %%a 2>nul
)
goto :eof

:: Function to check if a port is in use
:check_port
netstat -an | find ":%1" >nul
if %errorlevel% equ 0 (
    exit /b 0
) else (
    exit /b 1
)

:: Function to wait for a port to be available
:wait_for_port
set timeout=%2
set counter=0
:wait_loop
call :check_port %1
if %errorlevel% equ 0 (
    exit /b 0
)
timeout /t 1 /nobreak >nul
set /a counter+=1
if %counter% lss %timeout% goto wait_loop
echo Error: Port %1 is not available after %timeout% seconds
exit /b 1

:: Kill any existing processes on our ports
echo Checking for existing processes...
call :kill_port 5005
call :kill_port 4200

:: Start the API in the background
echo Starting LifeyLife API...
start /B cmd /c "cd LifeyLife.Api && set ASPNETCORE_ENVIRONMENT=Development && dotnet run"
set API_PID=%ERRORLEVEL%

:: Wait for API to start
echo Waiting for API to start...
call :wait_for_port 5005 30
if %errorlevel% neq 0 (
    echo Failed to start API
    taskkill /F /PID %API_PID% 2>nul
    exit /b 1
)

:: Start the frontend
echo Starting LifeyLife Frontend...
start /B cmd /c "cd LifeyLife.App\ClientApp && npm run start:dev"
set FRONTEND_PID=%ERRORLEVEL%

:: Wait for frontend to start
echo Waiting for frontend to start...
call :wait_for_port 4200 30
if %errorlevel% neq 0 (
    echo Failed to start frontend
    taskkill /F /PID %API_PID% 2>nul
    taskkill /F /PID %FRONTEND_PID% 2>nul
    exit /b 1
)

:: Handle script termination
:cleanup
echo Shutting down services...
taskkill /F /PID %API_PID% 2>nul
taskkill /F /PID %FRONTEND_PID% 2>nul
call :kill_port 5005
call :kill_port 4200
exit /b 0

:: Set up trap for script termination
:main
echo Development environment is running...
echo Press Ctrl+C to stop all services
echo API: http://localhost:5005
echo Frontend: http://localhost:4200

:: Keep the script running
:loop
timeout /t 1 /nobreak >nul
goto loop 