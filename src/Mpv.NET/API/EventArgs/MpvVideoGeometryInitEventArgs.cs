using System;
using System.Drawing;

namespace Mpv.NET.API
{
    public class MpvVideoGeometryInitEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public Rectangle Bounds { get; set; }
    }
}
