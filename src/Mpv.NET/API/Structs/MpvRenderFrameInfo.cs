using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    /// <summary>
    /// <para>Information about the next video frame that will be rendered. Can be</para>
    /// <para>retrieved with MPV_RENDER_PARAM_NEXT_FRAME_INFO.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvRenderFrameInfo
    {
        /// <summary>
        /// <para>A bitset of mpv_render_frame_info_flag values (i.e. multiple flags are</para>
        /// <para>combined with bitwise or).</para>
        /// </summary>
        public ulong flags;
        /// <summary>
        /// <para>Absolute time at which the frame is supposed to be displayed. This is in</para>
        /// <para>the same unit and base as the time returned by mpv_get_time_us(). For</para>
        /// <para>frames that are redrawn, or if vsync locked video timing is used (see</para>
        /// <para>"video-sync" option), then this can be 0. The "video-timing-offset"</para>
        /// <para>option determines how much "headroom" the render thread gets (but a high</para>
        /// <para>enough frame rate can reduce it anyway). mpv_render_context_render() will</para>
        /// <para>normally block until the time is elapsed, unless you pass it</para>
        /// <para>MPV_RENDER_PARAM_BLOCK_FOR_TARGET_TIME = 0.</para>
        /// </summary>
        public long target_time;
    }
}
