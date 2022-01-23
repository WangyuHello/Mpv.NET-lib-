using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API
{
    public struct MpvRenderFrameInfo
    {
        /**
        * A bitset of mpv_render_frame_info_flag values (i.e. multiple flags are
        * combined with bitwise or).
        */
        public ulong Flags;
        /**
         * Absolute time at which the frame is supposed to be displayed. This is in
         * the same unit and base as the time returned by mpv_get_time_us(). For
         * frames that are redrawn, or if vsync locked video timing is used (see
         * "video-sync" option), then this can be 0. The "video-timing-offset"
         * option determines how much "headroom" the render thread gets (but a high
         * enough frame rate can reduce it anyway). mpv_render_context_render() will
         * normally block until the time is elapsed, unless you pass it
         * MPV_RENDER_PARAM_BLOCK_FOR_TARGET_TIME = 0.
         */
        public ulong TargetTime;
    }
}
