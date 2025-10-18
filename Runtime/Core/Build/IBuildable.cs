namespace FLFloppa.Events
{
    /// <summary>
    /// Defines a contract for building configured instances at runtime.
    /// </summary>
    /// <typeparam name="T">Type of instance produced by the builder.</typeparam>
    public interface IBuildable<out T>
    {
        /// <summary>
        /// Builds a configured instance.
        /// </summary>
        /// <returns>A constructed instance of <typeparamref name="T"/>.</returns>
        T Build();
    }
}
