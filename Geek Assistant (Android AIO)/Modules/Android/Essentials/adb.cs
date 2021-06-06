﻿using System.Diagnostics;
using System.Linq;

internal static partial class adb {
    public static string adbOutput;
    /// <summary> 
    /// Sends a command to $"{GA_tools}\adb.exe" and waits for process output
    /// Do not include "adb" in the arguments parameter
    /// </summary>
    /// <param name="arguments">adb command arguments</param>
    /// <returns>Output of a adb command As String + adbOutput public string (to avoid repeating command for the same output)</returns>
    public static string Run(string arguments, Process adbProcess = null) {
        adbProcess ??= new();
        // >Failsafe - Should never happen
        if (string.IsNullOrEmpty(arguments)) {
            inf.detail.workCode += "-adbDo0"; // error code (last process) - adbDo 0 (no arguments)
            inf.Run(inf.lvls.FatalError, inf.workTitle, "Unable to run the adb command.");
        }

        // If Not adbIsReady(txt.GA_GetErrorInitials) Then
        // ErrorCodeTrack($"{S.ErrorCode}-adbD0") ' error code (last process) - adb device 0 (no device)
        // DoMsg(ErrorInfo.lvl, ErrorInfo.msg, 2)
        // End If
        // Inform if not running  
        if (Process.GetProcessesByName("adb").Length == 0)
            SetProgressText.Run(txt.RandomWorkText, -1);

        // <Failsafe
        {
            var adbPstartInfo = adbProcess.StartInfo;
            adbPstartInfo.FileName = $@"{c.GA_tools}\adb.exe";
            adbPstartInfo.Arguments = arguments;
            adbPstartInfo.UseShellExecute = false;
            adbPstartInfo.CreateNoWindow = true;
            adbPstartInfo.RedirectStandardOutput = true;
            adbPstartInfo.RedirectStandardInput = true;
            adbPstartInfo.RedirectStandardError = true;
        }
        // Start
        adbProcess.Start();
        adbProcess.WaitForExit();
        // Return as global string (avoid repeating command for output) 
        return adbOutput = adbProcess.StandardOutput.ReadToEnd();
    }
}