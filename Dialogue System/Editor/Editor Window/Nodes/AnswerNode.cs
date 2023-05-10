using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class AnswerNode : BaseDialogueNode
    {
        public AnswerNode() : base()
        {
            AnswerPorts = new ReadOnlyCollection<AnswerVariantPort>(_answerVariantPorts);
            InitAnswerControlButtons();
            InitInputPort();
            InitEditableTitleField();
            AddToClassList(AnserNodeUssClassName);
        }

        public AnswerNode(Vector2 position, string title) : this()
        {
            Rect rectPosition = new(position, Vector2.zero);
            SetPositionWithoutNotify(rectPosition);
            _editableTitleField.SetValueWithoutNotify(title);
        }

        public static readonly string AnserNodeUssClassName = "iup-answer-node";

        public string Title
        {
            get => _editableTitleField.value;
            set => _editableTitleField.value = value;
        }
        public InputDialoguePort InputPort { get; private set; }
        public ReadOnlyCollection<AnswerVariantPort> AnswerPorts { get; }

        private TextField _editableTitleField;
        private VisualElement _answerControlButtonsBlock;
        private readonly List<AnswerVariantPort> _answerVariantPorts = new();

        public event Action<AnswerNode, int> AnswerPortAdded;
        public event Action<AnswerNode, int, AnswerVariantPort> AnswerPortRemoved;
        public event Action<AnswerNode, int, int> AnswerPortPositionChanged;
        public event Action<AnswerNode, ChangeEvent<string>> TitleChanged;
        public event Action<AnswerNode, int, ChangeEvent<string>> AnswerPortTitleChanged;

        public void AddAnswerPort(string answerTitle = "")
        {
            AnswerVariantPort addedPort = new(Orientation.Horizontal, Port.Capacity.Single)
            {
                Title = answerTitle
            };
            addedPort.DeleteButtonClicked += DeleteAnswerPort;
            addedPort.MoveUpReorderButtonClicked += MoveAnswerVariantPortUp;
            addedPort.MoveDownReorderButtonClicked += MoveAnswerVariantPortDown;
            addedPort.TitleChanged += InvokeAnswerPortTitleChangedEvent;
            outputContainer.Insert(outputContainer.childCount - 1, addedPort);
            _answerVariantPorts.Add(addedPort);
            AnswerPortAdded?.Invoke(this, _answerVariantPorts.Count - 1);
        }

        public void DeleteAnswerPort(AnswerVariantPort deletedPort)
        {
            deletedPort.DeleteButtonClicked -= DeleteAnswerPort;
            deletedPort.MoveUpReorderButtonClicked -= MoveAnswerVariantPortUp;
            deletedPort.MoveDownReorderButtonClicked -= MoveAnswerVariantPortDown;
            deletedPort.TitleChanged -= InvokeAnswerPortTitleChangedEvent;
            outputContainer.Remove(deletedPort);
            int removedPortIndex = _answerVariantPorts.IndexOf(deletedPort);
            _ = _answerVariantPorts.Remove(deletedPort);
            AnswerPortRemoved?.Invoke(this, removedPortIndex, deletedPort);
        }

        private void InvokeAnswerPortTitleChangedEvent(
            AnswerVariantPort answerVariantPort,
            ChangeEvent<string> context)
        {
            int answerVariantPortIndex = _answerVariantPorts.IndexOf(answerVariantPort);
            AnswerPortTitleChanged?.Invoke(this, answerVariantPortIndex, context);
        }

        private void MoveAnswerVariantPortUp
            (AnswerVariantPort reorderAnswerVariantPort)
        {
            int reorderPortIndex = _answerVariantPorts.IndexOf(reorderAnswerVariantPort);
            if (reorderPortIndex > 0)
            {
                int newPortIndex = reorderPortIndex - 1;
                ChangeAnswerPortPosition(reorderPortIndex, newPortIndex);
            }
        }

        private void MoveAnswerVariantPortDown(
            AnswerVariantPort reorderAnswerVariantPort)
        {
            int reorderPortIndex = _answerVariantPorts.IndexOf(reorderAnswerVariantPort);
            int bottomAnswerPortIndex = _answerVariantPorts.Count - 1;
            if (reorderPortIndex < bottomAnswerPortIndex)
            {
                int newPortIndex = reorderPortIndex + 1;
                ChangeAnswerPortPosition(reorderPortIndex, newPortIndex);
            }
        }

        private void InvokeTitleChangedEvent(ChangeEvent<string> context)
        {
            TitleChanged?.Invoke(this, context);
        }

        private void ChangeAnswerPortPosition(int from, int to)
        {
            AnswerVariantPort reorderPort = _answerVariantPorts[from];
            _answerVariantPorts.MoveItemFromTo(from, to);
            outputContainer.RemoveAt(from);
            outputContainer.Insert(to, reorderPort);
            AnswerPortPositionChanged?.Invoke(this, from, to);
        }

        private void InitEditableTitleField()
        {
            _editableTitleField = NodeUtils.CreateNodeEditableTitleField("Answer Choice Node");
            _editableTitleField.RegisterValueChangedCallback(InvokeTitleChangedEvent);
            titleContainer.Insert(0, _editableTitleField);
        }

        private void InitAnswerControlButtons()
        {
            _answerControlButtonsBlock = new();
            Button addAnswerButton = new()
            {
                text = "+"
            };
            addAnswerButton.clicked += () => AddAnswerPort("Lotreamon");
            _answerControlButtonsBlock.Add(addAnswerButton);
            outputContainer.Add(_answerControlButtonsBlock);
        }

        private void InitInputPort()
        {
            InputPort = NodeUtils.CreateInputDialoguePort();
            inputContainer.Add(InputPort);
        }
    }
}
