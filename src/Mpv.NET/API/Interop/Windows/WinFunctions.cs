using System;
using System.Runtime.InteropServices;

namespace Mpv.NET.API.Interop
{
	internal static class WinFunctions
	{
		// https://docs.microsoft.com/en-us/windows/desktop/api/libloaderapi/nf-libloaderapi-loadlibrarya
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		// https://docs.microsoft.com/en-us/windows/desktop/api/libloaderapi/nf-libloaderapi-freelibrary
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
		public static extern int FreeLibrary(IntPtr hModule);

		// https://docs.microsoft.com/en-us/windows/desktop/api/libloaderapi/nf-libloaderapi-getprocaddress
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lProcName);

        public const int RTLD_LAZY = 1;
        public const int RTLD_GLOBAL = 8;

        [DllImport("libdl.so")]
        internal static extern IntPtr dlopen(string filename, int flags);

        [DllImport("libdl.so")]
        internal static extern IntPtr dlclose(IntPtr handle);

        [DllImport("libdl.so")]
        internal static extern IntPtr dlsym(IntPtr handle, string symbol);
    }
}