using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mpv.NET.API.Structs
{
    /// <summary>
    /// <para>For initializing the mpv OpenGL state via MPV_RENDER_PARAM_OPENGL_INIT_PARAMS.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvOpenglInitParams
    {
        /// <summary>
        /// <para>This retrieves OpenGL function pointers, and will use them in subsequent</para>
        /// <para>operation.</para>
        /// <para>Usually, you can simply call the GL context APIs from this callback (e.g.</para>
        /// <para>glXGetProcAddressARB or wglGetProcAddress), but some APIs do not always</para>
        /// <para>return pointers for all standard functions (even if present); in this</para>
        /// <para>case you have to compensate by looking up these functions yourself when</para>
        /// <para>libmpv wants to resolve them through this callback.</para>
        /// <para>libmpv will not normally attempt to resolve GL functions on its own, nor</para>
        /// <para>does it link to GL libraries directly.</para>
        /// </summary>
        public MpvOpenglInitParams_get_proc_addressCallback get_proc_address;
        /// <summary>
        /// <para>Value passed as ctx parameter to get_proc_address().</para>
        /// </summary>
        public void* get_proc_address_ctx;
    }
}
