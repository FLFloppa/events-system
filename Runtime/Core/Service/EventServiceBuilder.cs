using System;

namespace FLFloppa.Events
{
    public sealed class EventServiceBuilder
    {
        private readonly EventServiceConfiguration _configuration;

        public EventServiceBuilder(EventServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EventServiceBuilder(EventServiceConfigurationAsset asset)
        {
            if (asset is null) throw new ArgumentNullException(nameof(asset));
            _configuration = asset.Build();
        }

        public IEventService Build()
        {
            return EventServiceFactory.Create(_configuration);
        }

        public EventServiceConfiguration Configuration => _configuration;
    }
}
