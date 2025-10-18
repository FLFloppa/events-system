#nullable enable
using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Identifies a subscription configuration by name.
    /// </summary>
    public readonly struct SubscriptionKey : IEquatable<SubscriptionKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionKey"/> struct.
        /// </summary>
        /// <param name="value">Optional string value describing the subscription recipe.</param>
        public SubscriptionKey(string? value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the underlying string value for the key.
        /// </summary>
        public string? Value { get; }

        /// <inheritdoc />
        public bool Equals(SubscriptionKey other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is SubscriptionKey other && Equals(other);
        /// <inheritdoc />
        public override int GetHashCode() => Value != null ? Value.GetHashCode(StringComparison.Ordinal) : 0;
        /// <summary>
        /// Compares two keys for equality.
        /// </summary>
        public static bool operator ==(SubscriptionKey left, SubscriptionKey right) => left.Equals(right);
        /// <summary>
        /// Compares two keys for inequality.
        /// </summary>
        public static bool operator !=(SubscriptionKey left, SubscriptionKey right) => !left.Equals(right);
        /// <inheritdoc />
        public override string ToString() => Value ?? string.Empty;
    }
}
