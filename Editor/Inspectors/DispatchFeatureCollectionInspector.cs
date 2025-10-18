using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(DispatchFeatureCollection))]
    public sealed class DispatchFeatureCollectionInspector : UnityEditor.Editor
    {
        private SerializedProperty _bindings;
        private ReorderableList _bindingsList;

        private void OnEnable()
        {
            _bindings = serializedObject.FindProperty("bindings");
            _bindingsList = BuildBindingsList(_bindings, "Dispatch Feature Bindings");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Dispatch Feature Collection",
                "Aggregate reusable dispatch feature bindings for configurators."));

            root.Add(InspectorUi.Cards.Create(
                "Bindings",
                out var content,
                "Toggle bindings to include only the features required by this collection."));

            content.Add(new IMGUIContainer(() =>
            {
                serializedObject.Update();
                _bindingsList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }));

            return root;
        }

        private static ReorderableList BuildBindingsList(SerializedProperty property, string header)
        {
            var list = new ReorderableList(property.serializedObject, property, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, header),
                elementHeight = EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing,
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = property.GetArrayElementAtIndex(index);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, element.FindPropertyRelative("feature"), new GUIContent("Feature"));
                    rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(rect, element.FindPropertyRelative("enabled"), new GUIContent("Enabled"));
                }
            };

            return list;
        }
    }
}
