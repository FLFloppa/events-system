using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Dispatch feature that logs before and/or after payload delivery for diagnostics.
    /// </summary>
    public readonly struct DiagnosticsDispatchFeature<TEvent> : IDispatchFeature<TEvent> where TEvent : struct
    {
        private readonly string _label;
        private readonly bool _logBefore;
        private readonly bool _logAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsDispatchFeature{TEvent}"/> struct.
        /// </summary>
        /// <param name="label">Label prefixed to diagnostic logs.</param>
        /// <param name="logBefore">Whether to log prior to dispatch.</param>
        /// <param name="logAfter">Whether to log after dispatch.</param>
        public DiagnosticsDispatchFeature(string label, bool logBefore, bool logAfter)
        {
            _label = label;
            _logBefore = logBefore;
            _logAfter = logAfter;
        }

        /// <inheritdoc />
        public void BeforeDispatch(in TEvent payload, ref DispatchRuntimeState<TEvent> state)
        {
            if (_logBefore)
            {
                Debug.Log($"[{_label}] Before dispatch topic {state.TopicId}");
            }
        }

        /// <inheritdoc />
        public void AfterDispatch(in TEvent payload, ref DispatchRuntimeState<TEvent> state)
        {
            if (_logAfter)
            {
                Debug.Log($"[{_label}] After dispatch topic {state.TopicId}");
            }
        }
    }
}
