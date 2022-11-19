namespace LockStepEngine
{
    public enum ColliderLayerType
    {
        Static,
        Enemy,
        Hero,
        Count
    }
    
    public class HeroSystem : BaseSystem
    {
        public override void OnUpdate(LFloat deltaTime)
        {
            foreach (var player in gameStateService.GetPlayers())
            {
                player.OnUpdate(deltaTime);
            }
        }
    }
}