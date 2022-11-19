using System;
using UnityEditor;

namespace Framework
{
    public class EditorTools : Editor
    {
        public const string CTRL_ = "%";
        public const string SHIFT_ = "#";
        public const string ALT_ = "&";


        [MenuItem("Edit/ClearConsole " + ALT_ + "c", false, 37)]
        public static void ClearConsole()
        {
#if UNITY_2017 || UNITY_2018 ||UNITY_2019
        Type type = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
#else
            Type type = Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
#endif
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        /// <summary>
        /// F11键控制编辑器暂停和播放
        /// </summary>
        [MenuItem("Edit/暂停or开始 _F11", false)]
        static void PlayOrPause()
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }

        /// <summary>
        /// F11键控制编辑器暂停和播放
        /// </summary>
        [MenuItem("Edit/单帧运行 _F10", false)]
        static void StepFrame()
        {
            EditorApplication.Step();
        }
    }
}