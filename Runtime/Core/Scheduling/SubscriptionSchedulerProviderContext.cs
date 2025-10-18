using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Utility that manages a thread-safe ambient <see cref="ISubscriptionSchedulerProvider"/> scope.
    /// </summary>
    internal static class SubscriptionSchedulerProviderContext
    {
        private static readonly object Sync = new object();
        private static ISubscriptionSchedulerProvider _current = DefaultSubscriptionSchedulerProvider.Instance;

        /// <summary>
        /// Gets the provider currently in scope.
        /// </summary>
        public static ISubscriptionSchedulerProvider Current
        {
            get
            {
                lock (Sync)
                {
                    return _current;
                }
            }
        }

        /// <summary>
        /// Temporarily overrides the ambient provider until the returned scope is disposed.
        /// </summary>
        /// <param name="provider">Provider to use within the scope.</param>
        /// <returns>An <see cref="IDisposable"/> that restores the previous provider on dispose.</returns>
        public static IDisposable Use(ISubscriptionSchedulerProvider provider)
        {
            return new Scope(provider ?? DefaultSubscriptionSchedulerProvider.Instance);
        }

        private sealed class Scope : IDisposable
        {
            private readonly ISubscriptionSchedulerProvider _previous;
            private bool _disposed;

            /// <summary>
            /// Initializes a new scope and swaps in the provided scheduler provider.
            /// </summary>
            /// <param name="provider">Provider to set as current.</param>
            public Scope(ISubscriptionSchedulerProvider provider)
            {
                lock (Sync)
                {
                    _previous = _current;
                    _current = provider;
                }
            }

            /// <summary>
            /// Restores the previous provider when the scope completes.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                lock (Sync)
                {
                    if (!_disposed)
                    {
                        _current = _previous;
                        _disposed = true;
                    }
                }
            }
        }
    }
}
