using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System;
using System.Diagnostics;
using System.Management;
using DiskDetector;
using DiskDetector.Models;
using ff_utils_winforms.IO;

namespace ff_utils_winforms.OS
{
    class OSUtils
    {
        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
            return isAdmin;
        }

        public static Process SetStartInfo(Process proc, bool hidden, string filename = "cmd.exe")
        {
            proc.StartInfo.UseShellExecute = !hidden;
            proc.StartInfo.RedirectStandardOutput = hidden;
            proc.StartInfo.RedirectStandardError = hidden;
            proc.StartInfo.CreateNoWindow = hidden;
            proc.StartInfo.FileName = filename;
            return proc;
        }

        public static Process NewProcess(bool hidden, string filename = "cmd.exe")
        {
            Process proc = new Process();
            return SetStartInfo(proc, hidden, filename);
        }

        public static string GetCmdArg()
        {
            // TODO: Implement Config
            // bool stayOpen = Config.GetInt("cmdDebugMode") == 2;
            bool stayOpen = false;
            if (stayOpen)
                return "/K";
            else
                return "/C";
        }

        public static bool ShowHiddenCmd()
        {
            // TODO: Implement Config
            //return Config.GetInt("cmdDebugMode") > 0;
            return false;
        }

        public static bool DriveIsSSD(string path)
        {
            try
            {
                var detectedDrives = Detector.DetectFixedDrives(QueryType.SeekPenalty);
                if (detectedDrives.Count != 0)
                {
                    char pathDriveLetter = (path[0].ToString().ToUpper())[0];
                    foreach (var detectedDrive in detectedDrives)
                    {
                        if (detectedDrive.DriveLetter == pathDriveLetter && detectedDrive.HardwareType.ToString().ToLower().Trim() == "ssd")
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Failed to detect drive type: " + e.Message);
                return true;    // Default to SSD on fail
            }
            return false;
        }

        public static bool HasNonAsciiChars(string str)
        {
            return (Encoding.UTF8.GetByteCount(str) != str.Length);
        }
    }
}
