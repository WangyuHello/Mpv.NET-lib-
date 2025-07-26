using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Enums
{
    /// <summary>
    /// <para>Flags used in mpv_render_frame_info.flags. Each value represents a bit in it.</para>
    /// </summary>
    [Flags]
    public enum MpvRenderFrameInfoFlag
    {
        /// <summary>
        /// <para>Set if there is actually a next frame. If unset, there is no next frame</para>
        /// <para>yet, and other flags and fields that require a frame to be queued will</para>
        /// <para>be unset.</para>
        /// <para>This is set for _any_ kind of frame, even for redraw requests.</para>
        /// <para>Note that when this is unset, it simply means no new frame was</para>
        /// <para>decoded/queued yet, not necessarily that the end of the video was</para>
        /// <para>reached. A new frame can be queued after some time.</para>
        /// <para>If the return value of mpv_render_context_render() had the</para>
        /// <para>MPV_RENDER_UPDATE_FRAME flag set, this flag will usually be set as well,</para>
        /// <para>unless the frame is rendered, or discarded by other asynchronous events.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_PRESENT = 1,
        /// <summary>
        /// <para>If set, the frame is not an actual new video frame, but a redraw request.</para>
        /// <para>For example if the video is paused, and an option that affects video</para>
        /// <para>rendering was changed (or any other reason), an update request can be</para>
        /// <para>issued and this flag will be set.</para>
        /// <para>Typically, redraw frames will not be subject to video timing.</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_REDRAW = 2,
        /// <summary>
        /// <para>If set, this is supposed to reproduce the previous frame perfectly. This</para>
        /// <para>is usually used for certain "video-sync" options ("display-..." modes).</para>
        /// <para>Typically the renderer will blit the video from a FBO. Unset otherwise.</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_REPEAT = 4,
        /// <summary>
        /// <para>If set, the player timing code expects that the user thread blocks on</para>
        /// <para>vsync (by either delaying the render call, or by making a call to</para>
        /// <para>mpv_render_context_report_swap() at vsync time).</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_BLOCK_VSYNC = 8
    }
}
