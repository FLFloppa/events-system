using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(MainThreadExecutorAsset))]
    public sealed class MainThreadExecutorAssetInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Main Thread Executor Strategy",
                "Ensures event handlers execute on the Unity main thread."));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Use this executor when subscribers interact with Unity APIs that require the main thread.",
                HelpBoxMessageType.Info);

            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Pair with a background dispatcher if you need to offload heavy work before marshalling back to the main thread.",
                HelpBoxMessageType.Warning);

            return root;
        }
    }
}
