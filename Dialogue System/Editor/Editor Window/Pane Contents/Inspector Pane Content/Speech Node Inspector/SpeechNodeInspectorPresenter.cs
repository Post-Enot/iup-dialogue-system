using UnityEngine.Localization;
using SpeechNodeModel = IUP.Toolkits.DialogueSystem.SpeechNode;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class SpeechNodeInspectorPresenter : BaseInspectorPresenter
    {
        public SpeechNodeInspectorView InspectorView { get; private set; }
        public SpeechNodeModel InspectedSpeechNode { get; private set; }
        public DialogueTree EditableDialogueTree { get; private set; }

        public void SetInspectorView(SpeechNodeInspectorView inspectorView)
        {
            InspectorView = inspectorView;
        }

        public void SetEditableDialogueTree(DialogueTree editableDialogueTree)
        {
            EditableDialogueTree = editableDialogueTree;
        }

        public override void Repaint()
        {
            UnsubscribeFromViewChanges();
            InspectorView.NodeTitleField.SetValueWithoutNotify(InspectedSpeechNode.Title);
            InspectorView.SpeechStatementBlock.SetLocalizedStringWithoutNotify(
                InspectedSpeechNode.LocalizedString);
            InspectorView.SpeechStatementBlock.SetLocalizedAudioClipWithoutNotify(
                InspectedSpeechNode.LocalizedAudioClip);
            InitActorDropdownField(
                InspectorView.SpeechStatementBlock.ActorDropdownField,
                EditableDialogueTree.Actors,
                InspectedSpeechNode.Actor);
            SubscribeOnViewChanges();
        }

        public void SetInspectedSpeechNode(SpeechNodeModel speechNodeModel)
        {
            InspectedSpeechNode = speechNodeModel;
            Repaint();
        }

        private void SubscribeOnViewChanges()
        {
            InspectorView.SpeechStatementBlock.LocalizedStringChanged +=
                SpeechStatementBlock_LocalizedStringChanged;
        }

        private void UnsubscribeFromViewChanges()
        {
            InspectorView.SpeechStatementBlock.LocalizedStringChanged -=
                SpeechStatementBlock_LocalizedStringChanged;
        }

        private void SpeechStatementBlock_LocalizedStringChanged(LocalizedString localizedString)
        {
            InspectedSpeechNode.LocalizedString = localizedString;
        }
    }
}
