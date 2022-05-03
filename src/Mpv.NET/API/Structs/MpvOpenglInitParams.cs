using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MpvOpenglInitParams
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        GetProcAddressFn GetProcAddress;

        IntPtr GetProcAddressCtx;
    }

    public delegate void GetProcAddressFn(IntPtr ctx, string name);

}
