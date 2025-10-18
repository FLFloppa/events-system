using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(SequentialSubscriptionSchedulerAsset))]
    public sealed class SequentialSubscriptionSchedulerAssetInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Sequential Subscription Scheduler",
                "Subscribers execute strictly in the order they were registered."));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Sequential execution order (FIFO).",
                HelpBoxMessageType.Info);

            return root;
        }
    }
}
