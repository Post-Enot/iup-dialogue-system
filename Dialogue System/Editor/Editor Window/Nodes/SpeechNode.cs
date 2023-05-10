using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class SpeechNode : BaseDialogueNode
    {
        public new class UxmlFactory : UxmlFactory<SpeechNode, UxmlTraits> { }

        public SpeechNode() : base()
        {
            InitEditableTitleField();
            InitInputPort();
            InitOutputPort();
            InitStyleClasses();
        }

        public SpeechNode(Vector2 position, string title) : this()
        {
            Rect rectPosition = new(position, Vector2.zero);
            SetPositionWithoutNotify(rectPosition);
            _editableTitleField.SetValueWithoutNotify(title);
        }

        public static readonly string SpeechNodeUssClassName = "iup-speech-node";

        public string Title
        {
            get => _editableTitleField.value;
            set => _editableTitleField.value = value;
        }
        public InputDialoguePort InputPort { get; private set; }
        public OutputDialoguePort OutputPort { get; private set; }

        private TextField _editableTitleField;

        public event Action<SpeechNode, ChangeEvent<string>> TitleChanged;

        public void SetTitleWithoutNotify(string newTitle)
        {
            _editableTitleField.SetValueWithoutNotify(newTitle);
        }

        private void InitEditableTitleField()
        {
            _editableTitleField = NodeUtils.CreateNodeEditableTitleField("Speech Node");
            _editableTitleField.RegisterValueChangedCallback(InvokeTitleChangedEvent);
            titleContainer.Insert(0, _editableTitleField);
        }

        private void InvokeTitleChangedEvent(ChangeEvent<string> context)
        {
            TitleChanged?.Invoke(this, context);
        }

        private void InitInputPort()
        {
            InputPort = NodeUtils.CreateInputDialoguePort();
            inputContainer.Add(InputPort);
        }

        private void InitOutputPort()
        {
            OutputPort = NodeUtils.CreateOutputDialoguePort();
            outputContainer.Add(OutputPort);
        }

        private void InitStyleClasses()
        {
            AddToClassList(SpeechNodeUssClassName);
        }
    }
}
