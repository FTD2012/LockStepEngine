using System;
using System.Reflection;

namespace LockStepEngine
{
    public delegate void UICallback(object windowObject);
    
    public enum WindowDepthType
    {
        Normal,
        Notice,
        Forward,
    }

    public struct WindowCreateInfo
    {
        public string resDir;
        public WindowDepthType depthType;

        public WindowCreateInfo(string _resDir, WindowDepthType _depthType)
        {
            resDir = _resDir;
            depthType = _depthType;
        }
    }
    
    public interface IUIService : IService
    {
        bool IsDebugMode { get; }
        T GetService<T>() where T : IService;
        void RegisterAssembly(Assembly uiAssembly);
        void OpenWindow(string dir, WindowDepthType depth, UICallback callback = null);
        void OpenWindow(WindowCreateInfo info, UICallback callback = null);
        void CloseWindow(string dir);
        void CloseWindow(object window);
        void ShowDialog(string title, string body, Action<bool> resultCallback);
    }
}