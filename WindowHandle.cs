using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    public class WindowHandle
    {
        private IntPtr hWnd = IntPtr.Zero;

        private Process[] processes;
        public IntPtr GetHWnd() { return hWnd; }
        public void SetHWnd(IntPtr hWnd) { this.hWnd = hWnd;}


        public WindowHandle() {
            this.processes = GetProcesses();
        }

        public Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }


        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public IntPtr GetHWndByTitle(string winTitle)
        {
            return FindWindow(null, winTitle);
        }

        public IntPtr GetHWndByWindowTitleContains(string winTitleSubStr)
        {
            foreach (Process pList in processes)
            {
                if (pList.MainWindowTitle.Contains(winTitleSubStr))
                {
                    return pList.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }
        
        public IntPtr GetHWndByProcessName(string processName)
        {
            foreach (Process pList in processes)
            {
                if (StringUtils.EqualsIgnoreCase(pList.ProcessName, processName))
                    return pList.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        public List<IntPtr> GetWindowHandles()
        {
            List<IntPtr> hWnds = new List<IntPtr>();
            foreach (Process pList in processes)
                hWnds.Add(pList.MainWindowHandle);

            return hWnds;
        }


    }
}
