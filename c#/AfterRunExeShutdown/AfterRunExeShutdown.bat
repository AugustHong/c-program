@echo off
chcp 65001 >nul
REM 設定視窗大小，但位置不可控
REM 如果使用 工作排程器 要勾「不論使用者是否登入」
mode con: cols=15 lines=7 
setlocal

REM ====== 可調整參數 ======
set "MAX_WAIT=300"   REM 最多等待秒數（5 分鐘）就一定會執行關機
set "WAIT_AFTER=15" REM 執行完後等待幾秒再關機

REM ====== 你的 EXE 參數（在這裡改）======
REM 如果要從 bat 取到的話用 set "EXE_ARGS=%*" 即可
set "EXE_ARGS=5"

REM ====== 取得 bat 所在目錄 ======
set "BAT_DIR=%~dp0"
set "EXE_PATH=%BAT_DIR%AfterRunExeShutdown\bin\Debug\AfterRunExeShutdown.exe"

REM echo 執行程式: %EXE_PATH% %EXE_ARGS%

REM ====== 檢查 exe 是否存在 ======
if not exist "%EXE_PATH%" (
    echo 找不到 %EXE_PATH% ，取消關機
    pause
    exit /b
)

REM ====== 啟動並取得 PID ======
for /f "tokens=2 delims=," %%i in (
    'tasklist /fo csv ^| findstr /i "AfterRunExeShutdown.exe"'
) do set "OLD_PID=%%~i"

REM 執行程式(exe)
start "" "%EXE_PATH%" %EXE_ARGS%

REM 重新抓 PID（找新的）
set "PID="
for /f "tokens=2 delims=," %%i in (
    'tasklist /fo csv ^| findstr /i "AfterRunExeShutdown.exe"'
) do (
    if not "%%~i"=="!OLD_PID!" set "PID=%%~i"
)

echo 取得PID: %PID%

REM ====== 等待 exe 結束（或 timeout）======
set /a COUNT=0

:LOOP
tasklist /fi "PID eq !PID!" 2>nul | find "!PID!" >nul

if errorlevel 1 (
    echo 程式結束，往下
    goto DONE
)

if !COUNT! GEQ %MAX_WAIT% (
    echo 超時，強制關閉
    taskkill /f /pid !PID! >nul
    goto DONE
)

timeout /t 1 /nobreak >nul
set /a COUNT+=1
goto LOOP

:DONE

echo 等待 %WAIT_AFTER% 秒關機
timeout /t %WAIT_AFTER% /nobreak >nul

shutdown /s /t 0

endlocal