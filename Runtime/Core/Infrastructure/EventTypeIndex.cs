using System;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Generates deterministic identifiers for event payload types.
    /// </summary>
    internal static class EventTypeIndex
    {
        private static ushort _nextId = 1;
        private static readonly Dictionary<Type, ushort> Ids = new Dictionary<Type, ushort>();

        /// <summary>
        /// Gets a unique identifier for the specified event payload type.
        /// </summary>
        /// <typeparam name="TEvent">Struct payload type to identify.</typeparam>
        /// <returns>A stable identifier assigned to <typeparamref name="TEvent"/>.</returns>
        public static ushort GetId<TEvent>() where TEvent : struct
        {
            var type = typeof(TEvent);
            lock (Ids)
            {
                if (Ids.TryGetValue(type, out var existing))
                {
                    return existing;
                }

                var id = _nextId++;
                Ids[type] = id;
                return id;
            }
        }
    }
}
