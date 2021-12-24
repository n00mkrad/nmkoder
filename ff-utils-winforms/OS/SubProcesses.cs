using Nmkoder.Extensions;
using Nmkoder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nmkoder.OS
{
    class SubProcesses
    {
        static List<Process> subProcesses = new List<Process>();
        public static List<Process> AllSubProcesses { get { return GetStartedSubProcesses(); } }
        public static List<Process> RunningSubProcesses { get { return GetRunningSubProcesses(); } }
        public static List<Process> ExitedSubProcesses { get { return GetExitedSubProcesses(); } }

        public static void RegisterProcess(Process p)
        {
            subProcesses.Add(p);
        }

        public static List<Process> GetRunningSubProcesses ()
        {
            List<Process> running = new List<Process>();

            foreach(Process p in subProcesses)
            {
                try
                {
                    if (!p.HasExited)
                        running.Add(p);
                }
                catch { }
            }

            return running;
        }

        public static List<Process> GetExitedSubProcesses()
        {
            List<Process> running = new List<Process>();

            foreach (Process p in subProcesses)
            {
                try
                {
                    if (p.HasExited)
                        running.Add(p);
                }
                catch { }
            }

            return running;
        }

        public static List<Process> GetStartedSubProcesses()
        {
            List<Process> running = new List<Process>();

            foreach (Process p in subProcesses)
            {
                try
                {
                    running.Add(p);
                }
                catch { }
            }

            return running;
        }

        public static void ClearExitedProcesses ()
        {
            subProcesses = subProcesses.Where(x => !x.HasExited).ToList();
        }

        public static void KillAll ()
        {
            var running = RunningSubProcesses;
            Logger.Log($"SubProcesses: Killing {running.Count} subprocesses ({string.Join(", ", running.Select(x => x.StartInfo.FileName))})", true);

            foreach(Process p in running)
            {
                try
                {
                    OsUtils.KillProcessTree(p.Id);
                    Logger.Log($"SubProcesses: Killed process tree for {p.StartInfo.FileName} {p.StartInfo.Arguments.Trunc(150)}", true);
                }
                catch(Exception e)
                {
                    Logger.Log($"SubProcesses: Failed to kill process tree for {p.StartInfo.FileName} {p.StartInfo.Arguments.Trunc(150)}: {e.Message}", true);
                }
            }
        }
    }
}
