using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Memoria
{
    public class Mem
    {
        #region Importando pinvokes

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesWritten);

        #endregion

        #region Encontrando processo

        public Process proc;

        public Process GetProcess(string procname)
        {
            proc = Process.GetProcessesByName(procname)[0];
            return proc;
        }

        public IntPtr GetModuleBase(string modulename)
        {
            if (modulename.Contains(".exe"))
                return proc.MainModule.BaseAddress;

            foreach (ProcessModule module in proc.Modules)
            {
                if (module.ModuleName == modulename)
                    return module.BaseAddress;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region Lendo Memoria

        public IntPtr RPM(IntPtr baseAddress)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(proc.Handle, baseAddress, buffer, buffer.Length, IntPtr.Zero);

            return new IntPtr(BitConverter.ToInt32(buffer, 0));
        }

        public IntPtr RPM(IntPtr baseAddress, int offset)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(proc.Handle, IntPtr.Add(baseAddress, offset), buffer, buffer.Length, IntPtr.Zero);

            return new IntPtr(BitConverter.ToInt32(buffer, 0));
        }

        #endregion

        #region Lendo Bytes
        public byte[] ReadBytes(IntPtr baseAddress, int bytes)
        {
            byte[] buffer = new byte[bytes];
            ReadProcessMemory(proc.Handle, baseAddress, buffer, buffer.Length, IntPtr.Zero);

            return buffer;
        }

        public byte[] ReadBytes(IntPtr baseAddress, int offset, int bytes)
        {
            byte[] buffer = new byte[bytes];
            ReadProcessMemory(proc.Handle, IntPtr.Add(baseAddress, offset), buffer, buffer.Length, IntPtr.Zero);

            return buffer;
        }
        #endregion

        #region Escrevendo na Memoria
        public bool WPM(IntPtr address, byte[] newbytes)
        {
            return WriteProcessMemory(proc.Handle, address, newbytes, newbytes.Length, IntPtr.Zero);
        }

        public bool WPM(IntPtr address, int offset, byte[] newbytes)
        {
            return WriteProcessMemory(proc.Handle, IntPtr.Add(address, offset), newbytes, newbytes.Length, IntPtr.Zero);
        }
        #endregion
    }
}
