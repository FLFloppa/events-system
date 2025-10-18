using System;
using System.Collections.Generic;
using System.IO;
using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(EventServiceConfigurationAsset))]
    public sealed class EventServiceConfigurationAssetInspector : UnityEditor.Editor
    {
        private static readonly List<SubscriptionFeatureBinding> SubscriptionBindings = new();
        private static readonly List<DispatchFeatureBinding> DispatchBindings = new();

        private SerializedProperty _dispatchPolicy;
        private SerializedProperty _executorStrategy;
        private SerializedProperty _subscriptionConfigurator;
        private SerializedProperty _dispatchConfigurator;
        private SerializedProperty _diagnosticsProvider;
        private SerializedProperty _subscriptionScheduler;

        private HelpBox _validationBox;
        private InspectorUi.ExpandableLists.ListControl _subscriptionSummary;
        private InspectorUi.ExpandableLists.ListControl _dispatchSummary;
        private InspectorUi.ExpandableLists.ListControl _schedulerSummary;
        private Button _createDispatchPolicyButton;
        private Button _createExecutorButton;
        private Button _createSubscriptionConfiguratorButton;
        private Button _createDispatchConfiguratorButton;
        private Button _createDiagnosticsButton;
        private Button _createSchedulerButton;
        private IVisualElementScheduledItem _scheduledUpdate;

        private void OnEnable()
        {
            _dispatchPolicy = serializedObject.FindProperty("dispatchPolicy");
            _executorStrategy = serializedObject.FindProperty("executorStrategy");
            _subscriptionConfigurator = serializedObject.FindProperty("subscriptionConfigurator");
            _dispatchConfigurator = serializedObject.FindProperty("dispatchConfigurator");
            _diagnosticsProvider = serializedObject.FindProperty("diagnosticsProvider");
            _subscriptionScheduler = serializedObject.FindProperty("subscriptionScheduler");

            Undo.undoRedoPerformed += UpdateUIFromSerializedObject;
        }

        private void OnDisable()
        {
            _scheduledUpdate?.Pause();
            _scheduledUpdate = null;
            Undo.undoRedoPerformed -= UpdateUIFromSerializedObject;
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            var root = InspectorUi.Layout.CreateRoot();
            root.name = "event-service-configuration-inspector";

            root.Add(InspectorUi.Layout.CreateHeader(
                "FLFloppa Event Service Configuration",
                "Configure policies, configurators, diagnostics, and schedulers used to build the runtime event service."));

            var quickActionsCard = InspectorUi.Cards.Create(
                "Quick Actions",
                out var quickActionsContent,
                "Generate baseline collaborator assets in the same folder as this configuration.");
            quickActionsContent.Add(BuildCreationToolbar());
            root.Add(quickActionsCard);

            var coreCard = InspectorUi.Cards.Create(
                "Core Collaborators",
                out var coreContent,
                "Required assets for dispatching events and configuring subscriptions.");
            coreContent.Add(InspectorUi.Controls.CreatePropertyField(_dispatchPolicy, "Dispatch Policy", _ => UpdateUIFromSerializedObject()));
            coreContent.Add(InspectorUi.Controls.CreatePropertyField(_executorStrategy, "Executor Strategy", _ => UpdateUIFromSerializedObject()));
            coreContent.Add(InspectorUi.Controls.CreatePropertyField(_subscriptionConfigurator, "Subscription Configurator", _ => UpdateUIFromSerializedObject()));
            coreContent.Add(InspectorUi.Controls.CreatePropertyField(_dispatchConfigurator, "Dispatch Configurator", _ => UpdateUIFromSerializedObject()));
            root.Add(coreCard);

            var optionalCard = InspectorUi.Cards.Create(
                "Optional Collaborators",
                out var optionalContent,
                "Enhance runtime diagnostics and scheduling behaviour using optional assets.");
            optionalContent.Add(InspectorUi.Controls.CreatePropertyField(_diagnosticsProvider, "Diagnostics Provider", _ => UpdateUIFromSerializedObject()));
            optionalContent.Add(InspectorUi.Controls.CreatePropertyField(_subscriptionScheduler, "Subscription Scheduler", _ => UpdateUIFromSerializedObject()));
            root.Add(optionalCard);

            var summaryCard = InspectorUi.Cards.Create(
                "Configuration Summary",
                out var summaryContent,
                "Inspection of aggregated feature bindings and scheduler details.");

            _subscriptionSummary = InspectorUi.ExpandableLists.Create(
                "Subscription Features",
                "Resolved from the selected subscription configurator.");
            summaryContent.Add(_subscriptionSummary.Root);

            _dispatchSummary = InspectorUi.ExpandableLists.Create(
                "Dispatch Features",
                "Derived from configured dispatch features and bindings.");
            summaryContent.Add(_dispatchSummary.Root);

            _schedulerSummary = InspectorUi.ExpandableLists.Create(
                "Scheduler Details",
                "Information about the selected scheduler asset.");
            summaryContent.Add(_schedulerSummary.Root);

            root.Add(summaryCard);

            var validationCard = InspectorUi.Cards.Create(
                "Validation",
                out var validationContent,
                "Validation messages update automatically as properties change.");
            _validationBox = InspectorUi.Controls.AddHelpBox(validationContent, string.Empty, HelpBoxMessageType.Info);
            root.Add(validationCard);

            UpdateUI();

            _scheduledUpdate = root.schedule.Execute(UpdateUIFromSerializedObject).Every(500);

            return root;
        }

        private VisualElement BuildCreationToolbar()
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.Wrap,
                    justifyContent = Justify.SpaceBetween,
                    marginBottom = InspectorUi.Layout.SectionSpacing
                }
            };

            _createDispatchPolicyButton = InspectorUi.Controls.CreateActionButton("Create Dispatch Policy", () => CreateAsset<ImmediateDispatchPolicyAsset>("ImmediateDispatchPolicy", _dispatchPolicy));
            _createExecutorButton = InspectorUi.Controls.CreateActionButton("Create Executor", () => CreateAsset<MainThreadExecutorAsset>("MainThreadExecutor", _executorStrategy));
            _createSubscriptionConfiguratorButton = InspectorUi.Controls.CreateActionButton("Create Subscription Configurator", () => CreateAsset<FeatureListSubscriptionConfiguratorAsset>("SubscriptionConfigurator", _subscriptionConfigurator));
            _createDispatchConfiguratorButton = InspectorUi.Controls.CreateActionButton("Create Dispatch Configurator", () => CreateAsset<FeatureListDispatchConfiguratorAsset>("DispatchConfigurator", _dispatchConfigurator));
            _createDiagnosticsButton = InspectorUi.Controls.CreateActionButton("Create Diagnostics", () => CreateAsset<NullDiagnosticsProviderAsset>("DiagnosticsProvider", _diagnosticsProvider));
            _createSchedulerButton = InspectorUi.Controls.CreateActionButton("Create Scheduler", () => CreateAsset<SequentialSubscriptionSchedulerAsset>("SubscriptionScheduler", _subscriptionScheduler));

            var coreColumn = InspectorUi.Controls.CreateButtonColumn("Core", _createDispatchPolicyButton, _createExecutorButton, _createSubscriptionConfiguratorButton, _createDispatchConfiguratorButton);
            var optionalColumn = InspectorUi.Controls.CreateButtonColumn("Optional", _createDiagnosticsButton, _createSchedulerButton);
            optionalColumn.style.marginRight = 0f;

            row.Add(coreColumn);
            row.Add(optionalColumn);

            return row;
        }

        private void CreateAsset<TAsset>(string baseName, SerializedProperty property) where TAsset : ScriptableObject
        {
            var targetAsset = target as ScriptableObject;
            var targetPath = targetAsset != null ? AssetDatabase.GetAssetPath(targetAsset) : string.Empty;
            var directory = string.IsNullOrEmpty(targetPath) ? "Assets" : Path.GetDirectoryName(targetPath);
            if (string.IsNullOrEmpty(directory))
            {
                directory = "Assets";
            }

            var asset = ScriptableObject.CreateInstance<TAsset>();
            var assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(directory, $"{baseName}.asset"));

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            Undo.RecordObject(target, $"Assign {typeof(TAsset).Name}");
            serializedObject.Update();
            property.objectReferenceValue = asset;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);

            UpdateUIFromSerializedObject();
        }

        private void UpdateUIFromSerializedObject()
        {
            serializedObject.Update();
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateValidationMessage();
            UpdateSummaries();
            UpdateButtonStates();
        }

        private void UpdateValidationMessage()
        {
            if (_validationBox == null)
            {
                return;
            }

            var asset = (EventServiceConfigurationAsset)target;
            if (asset.TryValidate(out var message))
            {
                _validationBox.messageType = HelpBoxMessageType.Info;

                if (string.IsNullOrEmpty(message))
                {
                    message = "Configuration is valid.";
                }

                if (_diagnosticsProvider.objectReferenceValue == null)
                {
                    message += "\nDiagnostics provider not assigned. Null diagnostic provider will be used at runtime.";
                }

                _validationBox.text = message;
            }
            else
            {
                _validationBox.messageType = HelpBoxMessageType.Error;
                _validationBox.text = message;
            }
        }

        private void UpdateSummaries()
        {
            UpdateSubscriptionSummary();
            UpdateDispatchSummary();
            UpdateSchedulerSummary();
        }

        private void UpdateSubscriptionSummary()
        {
            if (_subscriptionSummary == null)
            {
                return;
            }

            _subscriptionSummary.ClearItems();
            var subscriptionAsset = _subscriptionConfigurator.objectReferenceValue as SubscriptionConfiguratorAsset;

            if (subscriptionAsset is FeatureListSubscriptionConfiguratorAsset listAsset)
            {
                SubscriptionBindings.Clear();
                listAsset.CollectActiveBindings(SubscriptionBindings);

                if (SubscriptionBindings.Count == 0)
                {
                    _subscriptionSummary.ShowEmptyState("No subscriptions configured.");
                    return;
                }

                foreach (var binding in SubscriptionBindings)
                {
                    var feature = binding.Feature;
                    var featureName = feature != null ? feature.name : "(missing feature)";
                    var state = binding.Enabled ? "Enabled" : "Disabled";
                    _subscriptionSummary.AddItem(featureName, state);
                }

                return;
            }

            if (subscriptionAsset != null)
            {
                _subscriptionSummary.AddItem(subscriptionAsset.name, subscriptionAsset.GetType().Name);
            }
            else
            {
                _subscriptionSummary.ShowEmptyState("No subscription configurator assigned.");
            }
        }

        private void UpdateDispatchSummary()
        {
            if (_dispatchSummary == null)
            {
                return;
            }

            _dispatchSummary.ClearItems();
            var dispatchAsset = _dispatchConfigurator.objectReferenceValue as DispatchConfiguratorAsset;

            if (dispatchAsset is FeatureListDispatchConfiguratorAsset listAsset)
            {
                DispatchBindings.Clear();
                listAsset.CollectActiveBindings(DispatchBindings);

                if (DispatchBindings.Count == 0)
                {
                    _dispatchSummary.ShowEmptyState("No dispatch features configured.");
                    return;
                }

                foreach (var binding in DispatchBindings)
                {
                    var feature = binding.Feature;
                    _dispatchSummary.AddItem(feature != null ? feature.name : "(missing feature)", binding.Enabled ? "Enabled" : "Disabled");
                }

                return;
            }

            if (dispatchAsset != null)
            {
                _dispatchSummary.AddItem(dispatchAsset.name, dispatchAsset.GetType().Name);
            }
            else
            {
                _dispatchSummary.ShowEmptyState("No dispatch configurator assigned.");
            }
        }

        private void UpdateSchedulerSummary()
        {
            if (_schedulerSummary == null)
            {
                return;
            }

            _schedulerSummary.ClearItems();

            var schedulerAsset = _subscriptionScheduler.objectReferenceValue as SubscriptionSchedulerAsset;
            if (schedulerAsset == null)
            {
                _schedulerSummary.AddItem("Sequential Scheduler", "Default in-order execution");
                return;
            }

            _schedulerSummary.AddItem(schedulerAsset.name, schedulerAsset.GetType().Name);

            switch (schedulerAsset)
            {
                case PrioritySubscriptionSchedulerAsset priority:
                    _schedulerSummary.AddItem("Default Priority", priority.DefaultPriority.ToString());
                    break;
                case ThrottledSubscriptionSchedulerAsset throttled:
                    _schedulerSummary.AddItem("Default Throttle", $"{throttled.DefaultThrottleSeconds:F2}s");
                    break;
                case BackpressureSubscriptionSchedulerAsset backpressure:
                    _schedulerSummary.AddItem("Backlog Limit", backpressure.DefaultBacklogLimit.ToString());
                    break;
            }
        }

        private void UpdateButtonStates()
        {
            if (_createDispatchPolicyButton == null)
            {
                return;
            }

            SetButtonState(_createDispatchPolicyButton, _dispatchPolicy.objectReferenceValue == null, "Dispatch policy asset is already assigned.");
            SetButtonState(_createExecutorButton, _executorStrategy.objectReferenceValue == null, "Executor strategy asset is already assigned.");
            SetButtonState(_createSubscriptionConfiguratorButton, _subscriptionConfigurator.objectReferenceValue == null, "Subscription configurator asset is already assigned.");
            SetButtonState(_createDispatchConfiguratorButton, _dispatchConfigurator.objectReferenceValue == null, "Dispatch configurator asset is already assigned.");
            SetButtonState(_createDiagnosticsButton, _diagnosticsProvider.objectReferenceValue == null, "Diagnostics provider already exists. Clear the field to create another.");
            SetButtonState(_createSchedulerButton, _subscriptionScheduler.objectReferenceValue == null, "Subscription scheduler already exists. Clear the field to create another.");
        }

        private static void SetButtonState(Button button, bool isEnabled, string disabledTooltip)
        {
            button.SetEnabled(isEnabled);
            button.tooltip = isEnabled ? "Creates a new asset next to the configuration file." : disabledTooltip;
        }
    }
}
