@echo off
setlocal

rem TaskManager をビルドして起動するスクリプト。
rem 使い方: run.cmd [Debug|Release]   (既定は Release)

set CONFIGURATION=%1
if "%CONFIGURATION%"=="" set CONFIGURATION=Release

call "%~dp0build.cmd" %CONFIGURATION%
if errorlevel 1 (
    echo ビルドに失敗したため起動を中止しました。
    exit /b 1
)

set EXE=%~dp0src\TaskManager.App\bin\%CONFIGURATION%\TaskManager.exe
if not exist "%EXE%" (
    echo 実行ファイルが見つかりません: %EXE%
    exit /b 1
)

echo アプリを起動します...
start "" "%EXE%"

endlocal
