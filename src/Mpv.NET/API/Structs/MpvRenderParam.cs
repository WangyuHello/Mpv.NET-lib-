using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API
{
    /// <summary>
    /// Used to pass arbitrary parameters to some mpv_render_* functions.The
    /// meaning of the data parameter is determined by the type, and each
    /// MPV_RENDER_PARAM_* documents what type the value must point to.
    ///
    /// Each value documents the required data type as the pointer you cast to
    /// void* and set on mpv_render_param.data.For example, if MPV_RENDER_PARAM_FOO
    /// documents the type as Something* , then the code should look like this:
    ///
    ///   Something foo = { ...};
    ///   mpv_render_param param;
    ///   param.type = MPV_RENDER_PARAM_FOO;
    ///   param.data = & foo;
    ///
    /// Normally, the data field points to exactly 1 object. If the type is char*,
    /// it points to a 0-terminated string.
    ///
    /// In all cases(unless documented otherwise) the pointers need to remain
    /// valid during the call only.Unless otherwise documented, the API functions
    /// will not write to the params array or any data pointed to it.
    ///
    ///
    /// As a convention, parameter arrays are always terminated by type==0. There
    /// is no specific order of the parameters required.The order of the 2 fields in
    ///
    /// this struct is guaranteed(even after ABI changes).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MpvRenderParam
    {
        public MpvRenderParamType Type;
        public IntPtr Data;
    }
}
