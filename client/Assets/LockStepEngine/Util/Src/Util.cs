namespace LockStepEngine
{
    // TODO: ljm >>> remove
    public static class UtilSystem
    {
        public static void StartService()
        {
            LTime.Init();
        }

        public static void UpdateService()
        {
            LTime.Update();
        }
    }
}