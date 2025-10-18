using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(FeatureListSubscriptionConfiguratorAsset))]
    public sealed class FeatureListSubscriptionConfiguratorAssetInspector : UnityEditor.Editor
    {
        private SerializedProperty _collections;
        private SerializedProperty _localBindings;
        private ReorderableList _collectionsList;
        private ReorderableList _bindingsList;

        private void OnEnable()
        {
            _collections = serializedObject.FindProperty("collections");
            _localBindings = serializedObject.FindProperty("localBindings");

            _collectionsList = BuildObjectList(_collections, "Collections");
            _bindingsList = BuildBindingList(_localBindings, "Local Bindings");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Feature List Subscription Configurator",
                "Combine subscription feature bindings from collections and local overrides."));

            root.Add(InspectorUi.Cards.Create(
                "Collection Sources",
                out var collectionsContent,
                "Collections centralise reusable subscription bindings shared across multiple configurators."));
            collectionsContent.Add(CreateListContainer(_collectionsList));

            root.Add(InspectorUi.Cards.Create(
                "Local Bindings",
                out var localContent,
                "Local bindings apply after collections, letting you add or override features."));
            localContent.Add(CreateListContainer(_bindingsList));

            return root;
        }

        private static ReorderableList BuildObjectList(SerializedProperty property, string header)
        {
            var list = new ReorderableList(property.serializedObject, property, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, header),
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = property.GetArrayElementAtIndex(index);
                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                }
            };
            return list;
        }

        private static ReorderableList BuildBindingList(SerializedProperty property, string header)
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

        private IMGUIContainer CreateListContainer(ReorderableList list)
        {
            return new IMGUIContainer(() =>
            {
                serializedObject.Update();
                list.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            });
        }
    }
}
