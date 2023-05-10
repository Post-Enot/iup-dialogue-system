using System;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class SpeechNodeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<SpeechNodeInspectorView, UxmlTraits> { }

        public SpeechNodeInspectorView()
        {
            InitNodeTitleField();
            InitSpeechStatementBlock();
        }

        public TextField NodeTitleField { get; private set; }
        public SpeechStatementBlock SpeechStatementBlock { get; private set; }

        public event Action<ChangeEvent<string>> TitleChanged;

        private void InitNodeTitleField()
        {
            NodeTitleField = new TextField()
            {
                label = "Title"
            };
            Add(NodeTitleField);
        }

        private void InitSpeechStatementBlock()
        {
            SpeechStatementBlock = CreateSpeechStatementBlock();
            Add(SpeechStatementBlock);
        }
    }
}
