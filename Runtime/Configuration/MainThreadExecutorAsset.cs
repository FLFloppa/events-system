using UnityEngine;

namespace FLFloppa.Events
{
    [CreateAssetMenu(
        fileName = "MainThreadExecutor",
        menuName = "FLFloppa/Events/Executor Strategies/Main Thread")]
    public sealed class MainThreadExecutorAsset : ExecutorStrategyAsset
    {
        public override IExecutorStrategy Build()
        {
            return MainThreadExecutorStrategy.Instance;
        }
    }
}
