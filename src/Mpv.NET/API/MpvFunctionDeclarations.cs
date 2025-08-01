﻿using Mpv.NET.API.Interop;
using Mpv.NET.API.Structs;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mpv.NET.API
{
	// Standard delegates
	// Taken from: https://github.com/mpv-player/mpv/blob/master/libmpv/client.h

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate long MpvClientAPIVersion();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler))]
	public delegate string MpvErrorString(MpvError error);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvFree(IntPtr data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler))]
	public delegate string MpvClientName(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr MpvCreate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvInitialise(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvDestroy(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvTerminateDestroy(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr MpvCreateClient(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler))]
		out string name);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvLoadConfigFile(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string fileName);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate long MpvGetTimeUs(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvSetOption(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		IntPtr data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvSetOptionString(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvCommand(IntPtr mpvHandle, IntPtr args);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvCommandAsync(IntPtr mpvHandle, ulong replyUserData, IntPtr args);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvSetProperty(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		IntPtr data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvSetPropertyString(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
	public delegate MpvError MpvSetPropertyAsync(
		IntPtr mpvHandle,
		ulong replyUserData,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		IntPtr data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvGetProperty(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		out IntPtr data);

	// Todo: Figure out how to free the pointer for the return value using mpv_free inside a custom marshaler. Until then
	// we'll have to keep the IntPtr. Same goes for the OSD variant below.
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr MpvGetPropertyString(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr MpvGetPropertyOSDString(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvGetPropertyAsync(
		IntPtr mpvHandle,
		ulong replyUserData,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvObserveProperty(
		IntPtr mpvHandle,
		ulong replyUserData,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int MpvUnobserveProperty(IntPtr mpvHandle, ulong registeredReplyUserData);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler))]
	public delegate string MpvEventName(MpvEventID eventID);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvRequestEvent(
		IntPtr mpvHandle,
		MpvEventID eventID,
		[MarshalAs(UnmanagedType.I1)]
		bool enable);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvRequestLogMessages(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string minLevel);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr MpvWaitEvent(IntPtr mpvHandle, double timeout);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvWakeup(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvSetWakeupCallback(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.FunctionPtr)]
		MpvWakeupCallback cb,
		IntPtr d);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int MpvGetWakeupPipe(IntPtr mpvHandle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void MpvWaitAsyncRequests(IntPtr mpvHandle);

	// Not strictly part of the C API but are used to invoke mpv_get_property with other value data types.

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvGetPropertyDouble(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		out double data);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate MpvError MpvGetPropertyLong(
		IntPtr mpvHandle,
		[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(MpvStringMarshaler), MarshalCookie = "free-com")]
		string name,
		MpvFormat format,
		out long data);

	// Other

	public delegate void MpvWakeupCallback(IntPtr d);

    public delegate void MpvD3dInitFn(IntPtr d3d11Device, IntPtr swapChain);
    public delegate void MpvRaCtxFn(IntPtr raCtx, IntPtr width, IntPtr height, IntPtr scalex, IntPtr scaley, IntPtr bounds_left, IntPtr bounds_top, IntPtr bounds_right, IntPtr bounds_bottom);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetD3DInitCallback(MpvD3dInitFn callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetRaCtxCallback(MpvRaCtxFn callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetPanelSize(IntPtr raCtx, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MpvSetPanelScale(IntPtr raCtx, float scaleX, float scaleY);

	// Render API
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int MpvRenderContextCreate(MpvRenderContext** res, IntPtr mpv, MpvRenderParam* @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int MpvRenderContextSetParameter(MpvRenderContext* ctx, MpvRenderParam @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int MpvRenderContextGetInfo(MpvRenderContext* ctx, MpvRenderParam @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvRenderContextSetUpdateCallback(MpvRenderContext* ctx, MpvRenderUpdateFn callback, IntPtr callback_ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ulong MpvRenderContextUpdate(MpvRenderContext* ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int MpvRenderContextRender(MpvRenderContext* ctx, MpvRenderParam* @params);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvRenderContextReportSwap(MpvRenderContext* ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvRenderContextFree(MpvRenderContext* ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MpvRenderUpdateFn(IntPtr cb_ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate IntPtr MpvOpenglInitParams_get_proc_addressCallback(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
}