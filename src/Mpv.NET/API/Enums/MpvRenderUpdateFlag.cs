using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API
{
    [Flags]
    public enum MpvRenderUpdateFlag
    {
        MPV_RENDER_UPDATE_FRAME = 1 << 0,
    }
}
