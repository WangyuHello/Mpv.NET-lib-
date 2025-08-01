﻿using Mpv.NET.API.Enums;
using Mpv.NET.API.Interop;
using Mpv.NET.API.Structs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mpv.NET.API
{
	public unsafe partial class Mpv : IDisposable
	{
		public IMpvFunctions Functions
		{
			get => functions;
			set
			{
				Guard.AgainstNull(value);

				functions = value;
			}
		}

		public IMpvEventLoop EventLoop
		{
			get => eventLoop;
			set
			{
				Guard.AgainstNull(value);

				if (!value.IsRunning)
					value.Start();

				eventLoop = value;
			}
		}

		public IntPtr Handle
		{
			get => handle;
			private set
			{
				if (value == IntPtr.Zero)
					throw new ArgumentException("Invalid handle pointer.", nameof(handle));

				handle = value;
			}
		}

        public IntPtr RaCtx
        {
            get => raCtx;
            private set
            {
                if (value == IntPtr.Zero)
                    throw new ArgumentException("Invalid handle pointer.", nameof(raCtx));

                raCtx = value;
            }
        }

		public MpvRenderContext* RenderCtx
		{
            get => renderCtx;
            private set
            {
                if (value == null)
                    throw new ArgumentException("Invalid handle pointer.", nameof(renderCtx));

                renderCtx = value;
            }
        }

		public MpvRenderUpdateFn UpdateCallback { get; set; }
        public MpvOpenglInitParams_get_proc_addressCallback GetProcAddress { get; set; }

        private IMpvFunctions functions;
		private IMpvEventLoop eventLoop;
		private IntPtr handle;
		private IntPtr raCtx;
		private MpvRenderContext* renderCtx;
        private MpvOpenglInitParams_get_proc_addressCallback _glGetProcAddress;
        private MpvRenderUpdateFn _mpvRenderUpdate;

        private bool disposed = false;

		public Mpv(string dllPath)
		{
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(dllPath, nameof(dllPath));

			Functions = new MpvFunctions(dllPath);

			InitialiseMpv();

            eventLoop = new MpvEventLoop(EventCallback, Handle, Functions);
			eventLoop.Start();
		}

		public Mpv(IMpvFunctions functions)
		{
			Functions = functions;

			InitialiseMpv();

			eventLoop = new MpvEventLoop(EventCallback, Handle, Functions);
		}

		public Mpv(IMpvFunctions functions, IMpvEventLoop eventLoop)
		{
			Functions = functions;

			EventLoop = eventLoop;

			InitialiseMpv();

			eventLoop = new MpvEventLoop(EventCallback, Handle, Functions);
			eventLoop.Start();
		}

		internal Mpv(IntPtr handle, IMpvFunctions functions)
		{
			Handle = handle;

			Functions = functions;

			eventLoop = new MpvEventLoop(EventCallback, Handle, Functions);
			eventLoop.Start();
		}

		private void InitialiseMpv()
		{
			Handle = Functions.Create();
			if (Handle == IntPtr.Zero)
				throw new MpvAPIException("Failed to create Mpv context.");

			var error = Functions.Initialise(Handle);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);
        }

        public void SetPanelSize(int width, int height)
		{
			if (RaCtx != IntPtr.Zero)
			{
                Functions.SetPanelSize(RaCtx, width, height);
            }
        }

        public void SetPanelScale(float scaleX, float scaleY)
        {
            if (RaCtx != IntPtr.Zero)
            {
                Functions.SetPanelScale(RaCtx, scaleX, scaleY);
            }
        }


        public void EnsureRenderContextCreated()
		{
			if(renderCtx == null)
			{
                _glGetProcAddress = GLGetProcAddress;
                _mpvRenderUpdate = MPVRenderUpdate;

                var MPV_RENDER_PARAM_API_TYPE_Data = Marshal.StringToHGlobalAnsi("opengl");

                var openglInitParams = new MpvOpenglInitParams
                {
                    get_proc_address = _glGetProcAddress,
                    get_proc_address_ctx = null
                };

                var MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data = Marshal.AllocHGlobal(Marshal.SizeOf<MpvOpenglInitParams>());
                Marshal.StructureToPtr(openglInitParams, MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data, false);

                var parameters = new MpvRenderParam[]
				{
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_API_TYPE, data = (void*)MPV_RENDER_PARAM_API_TYPE_Data},
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_OPENGL_INIT_PARAMS, data = (void*)MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data},
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_INVALID, data = null}
				};


                try
                {
                    MpvRenderContext* content = null;
                    fixed (MpvRenderParam* ptr = parameters)
                    {
                        var result = Functions.MpvRenderContextCreate(&content, handle, ptr);
                        if (result < 0)
                            throw new MpvAPIException("Failed to create new client.");
                    }
                    RenderCtx = content;

                    Functions.MpvRenderContextSetUpdateCallback(RenderCtx, _mpvRenderUpdate, IntPtr.Zero);
                }
                finally
                {
                    Marshal.FreeHGlobal(MPV_RENDER_PARAM_API_TYPE_Data);
                    Marshal.FreeHGlobal(MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data);
                }
            }
		}

        public void ReleaseRenderContext()
        {
            if (renderCtx == null) return;
            Functions.MpvRenderContextFree(renderCtx);
            renderCtx = null;
        }

        public void OpenGLRender(int width, int height, int fbo, int format = 0, int flipY = 0)
        {
            if (renderCtx == null || disposed) return;

            var fboArray = new MpvOpenglFbo[]
            {
                new(){ w = width, h = height, fbo = fbo, internal_format = format },
            };

            var flipYArray = new int[] { flipY };

            fixed (MpvOpenglFbo* fboPtr = fboArray)
            {
                fixed (int* flipYPtr = flipYArray)
                {
                    var parameters = new MpvRenderParam[]
                    {
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_OPENGL_FBO, data = fboPtr },
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_FLIP_Y, data = flipYPtr},
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_INVALID, data = null }
                    };

                    fixed (MpvRenderParam* renderParamPtr = parameters)
                    {
                        var result = Functions.MpvRenderContextRender(renderCtx, renderParamPtr);
                        if (result < 0)
                            throw new MpvAPIException("Failed to render.");
                    }
                }
            }
        }

        private IntPtr GLGetProcAddress(IntPtr ctx, string name)
        {
            if (GetProcAddress == null) return IntPtr.Zero;
            return GetProcAddress(ctx, name);
        }

        private void MPVRenderUpdate(IntPtr ctx)
        {
            if (UpdateCallback == null) return;
            UpdateCallback(ctx);
        }

        public long ClientAPIVersion()
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			return Functions.ClientAPIVersion();
		}

		public string ErrorString(MpvError error)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			return Functions.ErrorString(error);
		}

		public string ClientName()
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			return Functions.ClientName(Handle);
		}

		public Mpv CreateClient()
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			var newHandle = Functions.CreateClient(Handle, out string name);
			if (newHandle == IntPtr.Zero)
				throw new MpvAPIException("Failed to create new client.");

			return new Mpv(newHandle, Functions);
		}

		public void LoadConfigFile(string absolutePath)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			if (!Uri.TryCreate(absolutePath, UriKind.Absolute, out Uri _))
				throw new ArgumentException("Path is not absolute.");

			if (!File.Exists(absolutePath))
				throw new FileNotFoundException("Config file not found.");

			var error = Functions.LoadConfigFile(Handle, absolutePath);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, functions);
		}

		public long GetTimeUs()
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			return Functions.GetTimeUs(Handle);
		}

		[Obsolete("Semi-deprecated in favour of SetProperty. Very few options still need to be set via SetOption.")]
		public void SetOption(string name, byte[] data, MpvFormat format = MpvFormat.ByteArray)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));
			Guard.AgainstNull(data, nameof(data));

			var dataLength = data.Length;
			var dataPtr = Marshal.AllocCoTaskMem(dataLength);

			try
			{
				Marshal.Copy(data, 0, dataPtr, dataLength);

				var error = Functions.SetOption(Handle, name, format, dataPtr);
				if (error != MpvError.Success)
					throw MpvAPIException.FromError(error, Functions);
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
			}
		}

		[Obsolete("Semi-deprecated in favour of SetPropertyString. Very few options still need to be set via SetOptionString.")]
		public void SetOptionString(string name, string data)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));
			Guard.AgainstNull(data, nameof(data));

			var error = Functions.SetOptionString(Handle, name, data);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);
		}

		public void Command(params string[] args)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNull(args, nameof(args));

			if (args.Length < 1)
				throw new ArgumentException("Missing arguments.", nameof(args));

			var argsPtr = MpvMarshal.GetComPtrForManagedUTF8StringArray(args, out IntPtr[] argsPtrs);
			if (argsPtr == IntPtr.Zero)
				throw new MpvAPIException("Failed to convert string array to pointer array.");

			try
			{
				var error = Functions.Command(Handle, argsPtr);
				//var error = MpvFuntionsStatic.MpvCommand(Handle, argsPtr);

                if (error != MpvError.Success)
					throw MpvAPIException.FromError(error, Functions);
			}
			finally
			{
				MpvMarshal.FreeComPtrArray(argsPtrs);
				Marshal.FreeCoTaskMem(argsPtr);
			}
		}

		public void CommandAsync(ulong replyUserData, params string[] args)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNull(args, nameof(args));

			if (args.Length < 1)
				throw new ArgumentException("Missing arguments.", nameof(args));

			var argsPtr = MpvMarshal.GetComPtrForManagedUTF8StringArray(args, out IntPtr[] argsPtrs);
			if (argsPtr == IntPtr.Zero)
				throw new MpvAPIException("Failed to convert string array to pointer array.");

			try
			{
				var error = Functions.CommandAsync(Handle, replyUserData, argsPtr);
				if (error != MpvError.Success)
					throw MpvAPIException.FromError(error, Functions);
			}
			finally
			{
				MpvMarshal.FreeComPtrArray(argsPtrs);
				Marshal.FreeCoTaskMem(argsPtr);
			}
		}

		public void SetProperty(string name, byte[] data, MpvFormat format = MpvFormat.ByteArray)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));
			Guard.AgainstNull(data, nameof(data));

			if (data.Length < 1)
				throw new ArgumentException("Data is empty.", nameof(data));

			var dataLength = data.Length;
			var dataPtr = Marshal.AllocCoTaskMem(dataLength);

			try
			{
				Marshal.Copy(data, 0, dataPtr, dataLength);

				var error = Functions.SetProperty(Handle, name, format, dataPtr);
				//var error = MpvFuntionsStatic.MpvSetProperty(Handle, name, format, dataPtr);
                if (error != MpvError.Success)
					throw MpvAPIException.FromError(error, Functions);
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
			}
		}

		public void SetPropertyString(string name, string value)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(value, nameof(value));

			var error = Functions.SetPropertyString(Handle, name, value);
			//var error = MpvFuntionsStatic.MpvSetPropertyString(Handle, name, value);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);
		}

		public void SetPropertyAsync(string name, byte[] data, ulong replyUserData, MpvFormat format = MpvFormat.ByteArray)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));
			Guard.AgainstNull(data, nameof(data));

			if (data.Length < 1)
				throw new ArgumentException("Data is empty.", nameof(data));

			var dataLength = data.Length;
			var dataPtr = Marshal.AllocCoTaskMem(dataLength);

			try
			{
				Marshal.Copy(data, 0, dataPtr, dataLength);

				var error = Functions.SetPropertyAsync(Handle, replyUserData, name, format, dataPtr);
				if (error != MpvError.Success)
					throw MpvAPIException.FromError(error, Functions);
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
			}
		}

		public void SetPropertyLong(string name, long data)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var dataBytes = BitConverter.GetBytes(data);
			SetProperty(name, dataBytes, MpvFormat.Int64);
		}

		public void SetPropertyDouble(string name, double data)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var dataBytes = BitConverter.GetBytes(data);
			SetProperty(name, dataBytes, MpvFormat.Double);
		}

		public long GetPropertyLong(string name)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var error = Functions.GetPropertyLong(Handle, name, MpvFormat.Int64, out long value);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);

			return value;
		}

		public double GetPropertyDouble(string name)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var error = Functions.GetPropertyDouble(Handle, name, MpvFormat.Double, out double value);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);

			return value;
		}

		public string GetPropertyString(string name)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var stringPtr = Functions.GetPropertyString(Handle, name);
			if (stringPtr == IntPtr.Zero)
			{
                Debug.WriteLine($"Failed to get property {name}, invalid pointer.");
				return string.Empty;
            }

            try
			{
				return MpvMarshal.GetManagedUTF8StringFromPtr(stringPtr);
			}
			finally
			{
				Functions.Free(stringPtr);
			}
		}

		public void ObserveProperty(string name, MpvFormat format, ulong replyUserData)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));
			Guard.AgainstNullOrEmptyOrWhiteSpaceString(name, nameof(name));

			var error = Functions.ObserveProperty(Handle, replyUserData, name, format);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, functions);
		}

		public int UnobserveProperty(ulong registeredReplyUserData)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			var result = Functions.UnobserveProperty(Handle, registeredReplyUserData);
			if (result < 1)
			{
				var error = (MpvError)result;
				throw MpvAPIException.FromError(error, functions);
			}

			return result;
		}

		public string EventName(MpvEventID eventID)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			return Functions.EventName(eventID);
		}

		public void RequestEvent(MpvEventID eventID, bool enabled)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			var error = Functions.RequestEvent(Handle, eventID, enabled);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, functions);
		}

		public void RequestLogMessages(MpvLogLevel logLevel)
		{
			Guard.AgainstDisposed(disposed, nameof(Mpv));

			var stringLogLevel = MpvLogLevelHelper.GetStringForLogLevel(logLevel);

			var error = Functions.RequestLogMessages(Handle, stringLogLevel);
			if (error != MpvError.Success)
				throw MpvAPIException.FromError(error, Functions);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				// NOTE
				// The order of disposal is important here. Functions.TerminateDestroy is
				// responsible for disposing of unmanaged resources on mpv's side.
				// Inside the Dispose method of the MpvFunctions object, Windows.FreeLibrary
				// is used to free the resources of the loaded mpv DLL.
				// Windows.FreeLibrary MUST COME AFTER Functions.TerminarDestroy

				// The event loop calls into mpv so we can't TerminateDestrot yet!
				if (disposing && EventLoop is IDisposable disposableEventLoop)
					disposableEventLoop.Dispose();

				Functions.TerminateDestroy(Handle);

				if (disposing && Functions is IDisposable disposableFunctions)
					disposableFunctions.Dispose();

				disposed = true;
			}
		}

		~Mpv()
		{
			Dispose(false);
		}
	}
}