using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    /// <summary>
    /// <para>For MPV_RENDER_PARAM_DRM_DISPLAY_V2.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvOpenglDrmParamsV2
    {
        /// <summary>
        /// <para>DRM fd (int). Set to -1 if invalid.</para>
        /// </summary>
        public int fd;
        /// <summary>
        /// <para>Currently used crtc id</para>
        /// </summary>
        public int crtc_id;
        /// <summary>
        /// <para>Currently used connector id</para>
        /// </summary>
        public int connector_id;
        /// <summary>
        /// <para>Pointer to a drmModeAtomicReq pointer that is being used for the renderloop.</para>
        /// <para>This pointer should hold a pointer to the atomic request pointer</para>
        /// <para>The atomic request pointer is usually changed at every renderloop.</para>
        /// </summary>
        public DrmModeAtomicReq** atomic_request_ptr;
        /// <summary>
        /// <para>DRM render node. Used for VAAPI interop.</para>
        /// <para>Set to -1 if invalid.</para>
        /// </summary>
        public int render_fd;
    }
}
