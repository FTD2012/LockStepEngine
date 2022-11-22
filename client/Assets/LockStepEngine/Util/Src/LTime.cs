using System;
using System.Runtime.InteropServices;

namespace LockStepEngine
{
    // TODO: ljm >>> add to manager system
    public class Random
    {
        public ulong randSeed = 1;

        public Random(uint seed = 17)
        {
            randSeed = seed;
        }

        public uint Next()
        {
            randSeed = randSeed * 1103515245 + 36153;
            return (uint)(randSeed / 65535);
        }

        public uint Next(uint max)
        {
            return Next() % max;
        }

        public int Next(int max)
        {
            return (int)(Next() % max);
        }

        public uint Range(uint min, uint max)
        {
            if (min > max)
            {
                min = max;
            }

            uint num = max - min;
            return Next(num) + min;
        }

        public int Range(int min, int max)
        {
            if (min >= max - 1)
            {
                return min;
            }

            int num = max - min;
            return Next(num) + min;
        }
    }

    public class LRandom
    {
        private static Random i = new Random(3274);

        public static void SetSeed(uint seed)
        {
            i = new Random(seed);
        }

        public static uint Next()
        {
            return i.Next();
        }
        
        public static uint Next(uint max)
        {
            return i.Next(max);
        }

        public static int Next(int max)
        {
            return i.Next(max);
        }

        public static uint Range(uint min, uint max)
        {
            return i.Range(min, max);
        }

        public static int Range(int min, int max)
        {
            return i.Range(min, max);
        }
    }
    
    
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