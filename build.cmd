@echo off
setlocal

rem TaskManager をビルドしてテストを実行するスクリプト。
rem 使い方: build.cmd [Debug|Release]   (既定は Release)

set CONFIGURATION=%1
if "%CONFIGURATION%"=="" set CONFIGURATION=Release

set MSBUILD=

rem Visual Studio 2017 以降は vswhere で MSBuild を探す。
set VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if exist %VSWHERE% (
    for /f "usebackq tokens=*" %%i in (`%VSWHERE% -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set MSBUILD=%%i
)

rem 見つからない場合は .NET Framework 付属の MSBuild を使う。
if "%MSBUILD%"=="" (
    if exist "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" (
        set MSBUILD=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
    )
)

if "%MSBUILD%"=="" (
    echo MSBuild が見つかりませんでした。Visual Studio または Build Tools をインストールしてください。
    exit /b 1
)

echo MSBuild: %MSBUILD%
echo 構成: %CONFIGURATION%
echo.

rem NuGet パッケージを使っていないため、restore は不要。
"%MSBUILD%" "%~dp0TaskManager.sln" /t:Build /p:Configuration=%CONFIGURATION% /v:minimal /nologo
if errorlevel 1 (
    echo ビルドに失敗しました。
    exit /b 1
)

echo.
echo テストを実行します...
"%~dp0tests\TaskManager.Tests\bin\%CONFIGURATION%\TaskManager.Tests.exe"
if errorlevel 1 (
    echo テストに失敗しました。
    exit /b 1
)

echo.
echo 完了しました。実行ファイル: %~dp0src\TaskManager.App\bin\%CONFIGURATION%\TaskManager.exe
endlocal
