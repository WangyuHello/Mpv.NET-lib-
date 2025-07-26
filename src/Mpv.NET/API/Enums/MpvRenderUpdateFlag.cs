using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Enums
{
    /// <summary>
    /// <para>Flags returned by mpv_render_context_update(). Each value represents a bit</para>
    /// <para>in the function's return value.</para>
    /// </summary>
    public enum MpvRenderUpdateFlag
    {
        /// <summary>
        /// <para>A new video frame must be rendered. mpv_render_context_render() must be</para>
        /// <para>called.</para>
        /// </summary>
        MPV_RENDER_UPDATE_FRAME = 1
    }
}
