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
    class ProcessManager
    {
        static List<NmkoderProcess> subProcesses = new List<NmkoderProcess>();
        public static List<NmkoderProcess> AllSubProcesses { get { return GetStartedSubProcesses(); } }
        public static List<NmkoderProcess> RunningSubProcesses { get { return GetRunningSubProcesses(); } }
        public static List<NmkoderProcess> ExitedSubProcesses { get { return GetExitedSubProcesses(); } }

        public static void RegisterProcess(NmkoderProcess p)
        {
            subProcesses.Add(p);
        }

        public static List<NmkoderProcess> GetRunningSubProcesses ()
        {
            List<NmkoderProcess> running = new List<NmkoderProcess>();

            foreach(NmkoderProcess p in subProcesses)
            {
                try
                {
                    if (!p.Process.HasExited)
                        running.Add(p);
                }
                catch { }
            }

            return running;
        }

        public static List<NmkoderProcess> GetExitedSubProcesses()
        {
            List<NmkoderProcess> running = new List<NmkoderProcess>();

            foreach (NmkoderProcess p in subProcesses)
            {
                try
                {
                    if (p.Process.HasExited)
                        running.Add(p);
                }
                catch { }
            }

            return running;
        }

        public static List<NmkoderProcess> GetStartedSubProcesses()
        {
            List<NmkoderProcess> running = new List<NmkoderProcess>();

            foreach (NmkoderProcess p in subProcesses)
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
            subProcesses = subProcesses.Where(x => !x.Process.HasExited).ToList();
        }

        public static void Kill (List<NmkoderProcess> list)
        {
            Logger.Log($"SubProcesses: Killing {list.Count} subprocesses ({string.Join(", ", list.Select(x => x.Process.StartInfo.FileName))})", true);

            foreach(NmkoderProcess np in list)
            {
                Process p = np.Process;

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

        public static void KillAll()
        {
            Kill(RunningSubProcesses);
        }

        public static void KillPrimary ()
        {
            Kill(RunningSubProcesses.Where(x => x.Type == NmkoderProcess.ProcessType.Primary).ToList());
        }

        public static void KillSecondary()
        {
            Kill(RunningSubProcesses.Where(x => x.Type == NmkoderProcess.ProcessType.Secondary).ToList());
        }

        public static void KillBackground()
        {
            Kill(RunningSubProcesses.Where(x => x.Type == NmkoderProcess.ProcessType.Background).ToList());
        }
    }
}
