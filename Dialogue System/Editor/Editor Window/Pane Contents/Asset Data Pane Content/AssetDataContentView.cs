using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class AssetDataPaneContentView : PaneContentView
    {
        public AssetDataPaneContentView()
        {
            InitStyleClasses();
            InitAssetFoldout();
        }

        public static readonly string AssetDataPaneContentViewUssClassName = "iup-asset-data-pane-content-view";
        public static readonly string EditableAssetFieldUssClassName = "iup-editable-asset-field";
        public static readonly string SaveAssetButtonUssClassName = "iup-save-asset-button";

        public static readonly string AssetFoldoutText = "Asset";
        public static readonly string EditableAssetFieldLabel = "Editable Asset";
        public static readonly string SaveAssetButtonText = "Save Asset";

        public Foldout AssetFoldout { get; private set; }
        public ObjectField EditableAssetField { get; private set; }
        public Button SaveAssetButton { get; private set; }
        public Foldout ActorsFoldout { get; private set; }
        public Foldout BlackboardFoldout { get; private set; }

        public event Action SaveAssetButtonClicked;

        private void InitAssetFoldout()
        {
            AssetFoldout = new Foldout()
            {
                text = AssetFoldoutText
            };
            EditableAssetField = new ObjectField(EditableAssetFieldLabel)
            {
                objectType = typeof(DialogueTreeAsset)
            };
            SaveAssetButton = new Button(InvokeSaveAssetButtonClickedEvent)
            {
                text = SaveAssetButtonText
            };
            Add(AssetFoldout);
            AssetFoldout.Add(EditableAssetField);
            AssetFoldout.Add(SaveAssetButton);
        }

        private void InvokeSaveAssetButtonClickedEvent()
        {
            SaveAssetButtonClicked?.Invoke();
        }

        private void InitStyleClasses()
        {
            AddToClassList(AssetDataPaneContentViewUssClassName);
            EditableAssetField.AddToClassList(EditableAssetFieldUssClassName);
        }
    }
}
