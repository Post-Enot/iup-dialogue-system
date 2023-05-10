using AnswerNodeModel = IUP.Toolkits.DialogueSystem.AnswerNode;
using EdgeModel = IUP.Toolkits.DialogueSystem.DialogueEdge;
using EntryNodeModel = IUP.Toolkits.DialogueSystem.EntryNode;
using ExitNodeModel = IUP.Toolkits.DialogueSystem.ExitNode;
using SpeechNodeModel = IUP.Toolkits.DialogueSystem.SpeechNode;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class InspectorPresenter
    {
        public InspectorPaneContentView InspectorView { get; private set; }
        public DialogueTree EditableDialogueTree { get; private set; }
        public SpeechNodeInspectorPresenter SpeechNodeInspectorPresenter { get; } = new();
        public bool IsInspectorOpened { get; private set; }

        public void SetEditableDialogueTree(DialogueTree editableDialogueTree)
        {
            EditableDialogueTree = editableDialogueTree;
            SpeechNodeInspectorPresenter.SetEditableDialogueTree(EditableDialogueTree);
            CloseOpenedInspector();
        }

        public void SetView(InspectorPaneContentView inspectorView)
        {
            InspectorView = inspectorView;
            SpeechNodeInspectorPresenter.SetInspectorView(inspectorView.SpeechNodeInspectorView);
        }

        public void OpenEntryNodeInspector(EntryNodeModel entryNodeModel)
        {
            InspectorView.OpenEntryNodeInspector();
        }

        public void OpenSpeechNodeInspector(SpeechNodeModel speechNodeModel)
        {
            InspectorView.OpenSpeechNodeInspector();
            SpeechNodeInspectorPresenter.SetInspectedSpeechNode(speechNodeModel);
        }

        public void OpenAnswerNodeInspector(AnswerNodeModel answerNodeModel)
        {
            InspectorView.OpenAnswerNodeInspector();
        }

        public void OpenExitNodeInspector(ExitNodeModel exitNodeModel)
        {
            InspectorView.OpenExitNodeInspector();
        }

        public void OpenEdgeInspector(EdgeModel edgeModel)
        {
            InspectorView.OpenEdgeInspector();
        }

        public void CloseOpenedInspector()
        {
            InspectorView?.CloseOpenedInspector();
        }
    }
}
