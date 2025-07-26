using Mpv.NET.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    public unsafe struct MpvRenderParam
    {
        public MpvRenderParamType type;
        public void* data;
    }
}
