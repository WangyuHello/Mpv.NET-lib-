using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    /// <summary>
    /// <para>For MPV_RENDER_PARAM_DRM_DRAW_SURFACE_SIZE.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvOpenglDrmDrawSurfaceSize
    {
        /// <summary>
        /// <para>size of the draw plane surface in pixels.</para>
        /// </summary>
        public int width;
        /// <summary>
        /// <para>size of the draw plane surface in pixels.</para>
        /// </summary>
        public int height;
    }
}
