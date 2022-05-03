using Mpv.NET.API.Interop;
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
        public GetProcAddressFn GetProcAddress;

        public IntPtr GetProcAddressCtx;
    }

    public delegate IntPtr GetProcAddressFn(IntPtr ctx, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler))] string name);

}
