using System;

namespace LockStep.Util
{
    // TODO: ljm >>> add to manager system
    public static class LTime
    {
        private static DateTime initTime;
        public static DateTime InitTime
        {
            get
            {
                return initTime;
            }
        }
        
        private static DateTime lastFrameTime;
        public static DateTime LastFrameTime
        {
            get
            {
                return lastFrameTime;
            }
        }
        
        
        
        private static int frameCount;
        public static int FrameCount
        {
            get
            {
                return frameCount;
            }
        }

        private static float deltaTime;
        public static float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }
        
        private static float timeSinceLevelLoad;
        public static float TimeSinceLevelLoad
        {
            get
            {
                return timeSinceLevelLoad;
            }
        }
        
        private static float realTimeSinceStartUp;
        public static float RealTimeSinceStartUp
        {
            get
            {
                return realTimeSinceStartUp;
            }
        }
        
        private static long realTimeSinceStartUpMS;
        public static long RealTimeSinceStartUpMS
        {
            get
            {
                return realTimeSinceStartUpMS;
            }
        }

        public static void Init()
        {
            initTime = DateTime.Now;
        }

        public static void Update()
        {
            var now = DateTime.Now;
            deltaTime = (float) (now - lastFrameTime).TotalSeconds;
            timeSinceLevelLoad = (float) (now - initTime).TotalSeconds;
            realTimeSinceStartUp = (float) (now - initTime).TotalSeconds;
            realTimeSinceStartUpMS = (long) (now - initTime).TotalMilliseconds;
            lastFrameTime = now;
            frameCount++;
        }
    }
}