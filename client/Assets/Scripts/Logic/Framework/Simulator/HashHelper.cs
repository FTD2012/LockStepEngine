using System.Collections.Generic;

namespace LockStepEngine
{
    public class HashHelper : BaseSimulatorHelper
    {
        private INetworkService networkService;
        private IFrameBuffer cmdBuffer;
        private List<int> allHashCodes = new List<int>();
        private int firstHashTick;
        private Dictionary<int, int> tick2Hash = new Dictionary<int, int>();

        public HashHelper(IServiceContainer _serviceContainer, World _world, INetworkService _networkService, IFrameBuffer _cmdBuffer) : base(_serviceContainer, _world)
        {
            networkService = _networkService;
            cmdBuffer = _cmdBuffer;
        }

        public void CheckAndSendHashCodes()
        {
            if (cmdBuffer.NextTickToCheck > firstHashTick)
            {
                var count = LMath.Min(allHashCodes.Count, (cmdBuffer.NextTickToCheck) - firstHashTick, (480/4));
                if (count > 0)
                {
                    networkService.SendHashCodeList(firstHashTick, allHashCodes, 0, count);
                    firstHashTick += count;
                    allHashCodes.RemoveRange(0, count);
                }
            }
        }

        public bool TryGetValue(int tick, out int hash)
        {
            return tick2Hash.TryGetValue(tick, out hash);
        }

        public int CalcHash(bool isNeedTrace = false)
        {
            int idx = 0;
            return CalcHash(ref idx, isNeedTrace);
        }
        
        private int CalcHash(ref int idx, bool isNeedTrace)
        {
            int hashIdx = 0;
            int hashCode = 0;
            var debug = serviceContainer.Get<IDebugService>();
            foreach (var service in serviceContainer.GetAll())
            {
                if (service is IHashCode hashService)
                {
                    hashCode += hashService.GetHashCode(ref hashIdx) * PrimerLUT.GetPrimer(hashIdx++);
                    if (isNeedTrace)
                    {
                        debug.Trace($"svc {service.GetType().Name} hashCode{hashCode}", true);
                    }
                }
            }

            return hashCode;
        }

        public void SetHash(int tick, int hash)
        {
            if (tick < firstHashTick)
            {
                return;
            }

            var idx = tick - firstHashTick;
            if (allHashCodes.Count <= idx)
            {
                for (int i = 0; i < idx + 1; i++)
                {
                    allHashCodes.Add(0);
                }
            }

            allHashCodes[idx] = hash;
            tick2Hash[Tick] = hash;
        }
    }
}