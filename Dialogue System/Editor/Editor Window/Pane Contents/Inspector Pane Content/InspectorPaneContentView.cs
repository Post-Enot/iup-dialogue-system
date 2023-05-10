using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class InspectorPaneContentView : PaneContentView
    {
        public new class UxmlFactory : UxmlFactory<InspectorPaneContentView, UxmlTraits> { }

        public InspectorPaneContentView()
        {
            InitStyleClasses();
        }

        public static readonly string InspectorViewUssClassName = "iup-inspector-pane-content-view";

        public bool IsInspectorOpened => _openedInspector != null;
        public EntryNodeInspectorView EntryNodeInspectorView { get; private set; } = new();
        public SpeechNodeInspectorView SpeechNodeInspectorView { get; private set; } = new();
        public AnswerNodeInspectorView AnswerNodeInspectorView { get; private set; } = new();
        public ExitNodeInspectorView ExitNodeInspectorView { get; private set; } = new();
        public EdgeInspectorView EdgeInspectorView { get; private set; } = new();

        private InspectorView _openedInspector;

        public void OpenEntryNodeInspector()
        {
            OpenInspector(EntryNodeInspectorView);
        }

        public void OpenSpeechNodeInspector()
        {
            OpenInspector(SpeechNodeInspectorView);
        }

        public void OpenAnswerNodeInspector()
        {
            OpenInspector(AnswerNodeInspectorView);
        }

        public void OpenExitNodeInspector()
        {
            OpenInspector(ExitNodeInspectorView);
        }

        public void OpenEdgeInspector()
        {
            OpenInspector(EdgeInspectorView);
        }

        public void CloseOpenedInspector()
        {
            if (IsInspectorOpened)
            {
                Remove(_openedInspector);
                _openedInspector = null;
            }
        }

        private void OpenInspector(InspectorView inspectorView)
        {
            if (IsInspectorOpened && (_openedInspector != inspectorView))
            {
                CloseOpenedInspector();
            }
            Add(inspectorView);
            _openedInspector = inspectorView;
        }

        private void InitStyleClasses()
        {
            AddToClassList(InspectorViewUssClassName);
        }
    }
}
