using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FLFloppa.Events.Samples
{
    public sealed class EventServiceQuickStartSample : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private EventServiceConfigurationAsset configuration;

        private IEventService _service;
        private SubscriptionHandle<PlayerScored> _playerScoredHandle;

        private void Awake()
        {
            if (configuration == null)
            {
                Debug.LogError("EventServiceQuickStartSample requires a configuration asset.");
                enabled = false;
                return;
            }

            _service = new EventServiceBuilder(configuration).Build();
        }

        private void OnEnable()
        {
            if (_service == null)
            {
                return;
            }

            var recipe = configuration.SubscriptionConfigurator.Create<PlayerScored>();
            _playerScoredHandle = _service.Subscribe(recipe, OnPlayerScored);

            _service.Publish(new PlayerScored { Amount = 5 });
            UniTask.Void(async () => await _service.PublishAsync(new PlayerScored { Amount = 10 }, DispatchRecipe<PlayerScored>.Empty));
        }

        private void OnDisable()
        {
            if (_service == null || !_playerScoredHandle.IsValid)
            {
                return;
            }

            _service.Unsubscribe(_playerScoredHandle);
        }

        private void OnPlayerScored(in PlayerScored evt)
        {
            Debug.Log($"[EventServiceQuickStartSample] Player scored {evt.Amount} points!");
        }

        [Serializable]
        public struct PlayerScored
        {
            public int Amount;
        }
    }
}
