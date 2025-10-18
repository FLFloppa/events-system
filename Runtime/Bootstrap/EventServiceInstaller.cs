#nullable enable
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// MonoBehaviour that provisions an <see cref="IEventService"/> instance from a configuration asset.
    /// </summary>
    public sealed class EventServiceInstaller : MonoBehaviour
    {
        /// <summary>
        /// Optional configuration asset used to build the event service when initializing.
        /// </summary>
        [SerializeField] private EventServiceConfigurationAsset? configuration;
        /// <summary>
        /// When enabled, initializes the service automatically during <see cref="Awake"/> or first access.
        /// </summary>
        [SerializeField] private bool autoInitialize = true;

        private IEventService? _service;

        /// <summary>
        /// Gets the provisioned event service, initializing it automatically when configured to do so.
        /// </summary>
        public IEventService Service
        {
            get
            {
                if (_service == null && autoInitialize)
                {
                    Initialize();
                }

                return _service ?? throw new System.InvalidOperationException("Event Service has not been initialized.");
            }
        }

        /// <summary>
        /// Invoked by Unity. Initializes the service automatically when configured.
        /// </summary>
        private void Awake()
        {
            if (autoInitialize)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initializes the installer by creating an event service from the configuration asset.
        /// Subsequent calls are ignored once the service is initialized.
        /// </summary>
        public void Initialize()
        {
            if (_service != null)
            {
                return;
            }

            if (configuration == null)
            {
                throw new System.InvalidOperationException("Event Service configuration asset reference is missing.");
            }

            _service = EventServiceFactory.Create(configuration);
        }

        /// <summary>
        /// Invoked by Unity when the component is being destroyed. Clears the cached service reference.
        /// </summary>
        private void OnDestroy()
        {
            _service = null;
        }
    }
}
