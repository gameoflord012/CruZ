using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine
{

    public partial class GameApplication
    {
        public static event Action<Viewport>? WindowResized;
        public static event Action? Initialized;
        public static event Action? Exiting;
        public static event Action? AfterDraw;

        public static void CheckThread()
        {
            Trace.Assert(Instance._threadId == Environment.CurrentManagedThreadId);
        }

        public static GameApplication CreateContext(GameWrapper core, string gameResourceDir)
        {
            if(_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Dispose needed before creating new context");

            return _instance = new GameApplication(core, gameResourceDir);
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return Instance.GraphicsDevice;
        }

        public static MarshalInvokeResult MarshalInvoke(Action action, bool forwardException = false)
        {
            Instance.GenerateMarshalRequest(action, forwardException, out MarshalInvokeResult invokeResult);
            return invokeResult;
        }

        internal static AutoResizeRenderTarget CreateRenderTarget()
        {
            var rt = new AutoResizeRenderTarget(Instance.GraphicsDevice, Instance.Window);
            Instance._disposables.Add(rt);
            return rt;
        }

        private static T CheckNull<T>(T? value)
        {
            return value ?? throw new InvalidOperationException("Set value first");
        }

        public static bool IsActive()
        {
            return Instance._wrapper.IsActive;
        }

        public static ContentManager GetContentManager()
        {
            return Instance.Content;
        }

        private static GameApplication Instance
        {
            get => CheckNull(_instance);
        }

        public static string GameResourceDir
        {
            get => CheckNull(Instance._gameResourceDir);
        }

        public static ResourceManager Resource
        {
            get => CheckNull(Instance._gameResource);
        }

        public static ResourceManager InternalResource
        {
            get => CheckNull(Instance._internalResource);
        }

        public static Camera MainCamera
        {
            get => Instance._mainCamera;
        }

        public static Func<AssemblyName, Assembly?> AssemblyResolver
        {
            get => (resolvingAss) => AppDomain.CurrentDomain.GetAssemblies()
                    .First(domainAss => domainAss.FullName == resolvingAss.FullName);
        }

        private static GameApplication? _instance;
    }
}
