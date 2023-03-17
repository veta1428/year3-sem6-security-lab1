using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SecurityReport.Blocks;
internal class RamPropertiesRetriever : IDataRetriever
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }

    private readonly List<string> desiredProperties = new List<string>()
    {
        "DWORD Length",
        "Memory Load",
        "Total Physical Memory",
        "Available Physical Memory",
        "Total Page File Memory",
        "Available Page File Memory",
        "Total Virtual Memory",
        "Available Virtual Memory"
    };


    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    public List<Data.Data> RetrieveData()
    {
        List<Data.Data> result = new List<Data.Data>();
        MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
        if (GlobalMemoryStatusEx(memStatus))
        {
            result.Add(new Data.Data(desiredProperties[0], memStatus.dwLength.ToString()));
            result.Add(new Data.Data(desiredProperties[1], memStatus.dwMemoryLoad.ToString()));
            result.Add(new Data.Data(desiredProperties[2], memStatus.ullTotalPhys.ToString()));
            result.Add(new Data.Data(desiredProperties[3], memStatus.ullAvailPhys.ToString()));
            result.Add(new Data.Data(desiredProperties[4], memStatus.ullTotalPageFile.ToString()));
            result.Add(new Data.Data(desiredProperties[5], memStatus.ullAvailPageFile.ToString()));
            result.Add(new Data.Data(desiredProperties[6], memStatus.ullTotalVirtual.ToString()));
            result.Add(new Data.Data(desiredProperties[7], memStatus.ullAvailVirtual.ToString()));
        }
        return result;
    }
}
