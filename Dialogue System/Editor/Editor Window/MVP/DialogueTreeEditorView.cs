using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using UnityObject = UnityEngine.Object;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class DialogueTreeEditorView
    {
        public DialogueTreeEditorView(VisualElement rootVisualElement)
        {
            RootVisualElement = rootVisualElement;
            InitDialogueTreeView();
            InitInspectorPaneContentView();
        }

        public VisualElement RootVisualElement { get; }
        public DialogueTreeView DialogueTreeView { get; private set; }
        public InspectorPaneContentView InspectorPaneContentView { get; private set; }
        public ActorsListPresenter ActorsListView { get; private set; }

        private ObjectField _editableAssetField;
        private Button _applyChangesButton;

        public event Action<DialogueTreeAsset> EditableAssetChanged;
        public event Action ApplyChangesButtonClicked;

        public void SetEditableAssetFieldValueWithoutNotify(DialogueTreeAsset dialogueTreeAsset)
        {
            _editableAssetField.SetValueWithoutNotify(dialogueTreeAsset);
        }

        private void InitDialogueTreeView()
        {
            DialogueTreeView = RootVisualElement.Q<DialogueTreeView>("dialogue-tree-view");

            _editableAssetField = RootVisualElement.Q<ObjectField>("editable-asset-field");
            _editableAssetField.objectType = typeof(DialogueTreeAsset);
            _editableAssetField.RegisterValueChangedCallback(InvokeEditableAssetChangedEvent);

            _applyChangesButton = RootVisualElement.Q<Button>("apply-changes-button");
            _applyChangesButton.clicked += InvokeApplyChangesButtonClickedEvent;

            ListView actorListView = RootVisualElement.Q<ListView>("actors-list-view");
            ActorsListView = new ActorsListPresenter(actorListView);
        }

        private void InitInspectorPaneContentView()
        {
            InspectorPaneContentView = RootVisualElement.Q<InspectorPaneContentView>("inspector-pane-content-view");
        }

        private void InvokeEditableAssetChangedEvent(ChangeEvent<UnityObject> context)
        {
            EditableAssetChanged?.Invoke(context.newValue as DialogueTreeAsset);
        }

        private void InvokeApplyChangesButtonClickedEvent()
        {
            ApplyChangesButtonClicked?.Invoke();
        }
    }
}
