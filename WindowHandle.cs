using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        
        public IntPtr GetHWndByWindowTitleContains(string winTitleSubStr)
        {
            foreach (Process pList in processes)
            {
                if (pList.MainWindowTitle.Contains("Skeleton"))
                {
                    return pList.MainWindowHandle;
                }
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
