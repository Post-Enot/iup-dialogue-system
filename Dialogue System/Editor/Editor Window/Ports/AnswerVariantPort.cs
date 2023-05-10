using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class AnswerVariantPort : OutputDialoguePort
    {
        public AnswerVariantPort(Orientation orientation, Capacity capacity) :
            base(orientation, capacity)
        {
            InitEditableTitleField();
            InitButtonsBlock();
            InitStyleClasses();
        }

        public static readonly string AnswerVariantPortUssClassName = "iup-answer-variant-port";
        public static readonly string AnswerVariantPortEditableTitleFieldUssClassName =
            "iup-answer-variant-port__editable-title-field";
        public static readonly string AnswerVariantPortButtonContainerUssClassName =
            "iup-answer-variant-port__button-container";
        public static readonly string AnswerVariantPortDeleteButtonUssClassName =
            "iup-answer-variant-port__delete-button";
        public static readonly string ReorderButtonsContainerUssClassName =
            "iup-answer-variant-port__reorder-buttons-container";
        public static readonly string MoveUpReorderButtonUssClassName =
            "iup-answer-variant-port__move-up-reorder-button";
        public static readonly string MoveDownReorderButtonUssClassName =
            "iup-answer-variant-port__move-down-reorder-button";
        public static readonly string ReorderButtonUssClassName =
            "iup-answer-variant-port__reorder-button";

        public string Title
        {
            get => _editableTitleField.value;
            set => _editableTitleField.value = value;
        }

        private VisualElement _buttonContainer;
        private VisualElement _reorderButtonsContainer;
        private Button _moveUpReorderButton;
        private Button _moveDownReorderButton;
        private Button _deleteButton;
        private TextField _editableTitleField;

        public event Action<AnswerVariantPort> DeleteButtonClicked;
        public event Action<AnswerVariantPort> MoveUpReorderButtonClicked;
        public event Action<AnswerVariantPort> MoveDownReorderButtonClicked;
        public event Action<AnswerVariantPort, ChangeEvent<string>> TitleChanged;

        public void SetTitleWithoutNotify(string newTitle)
        {
            _editableTitleField.SetValueWithoutNotify(newTitle);
        }

        private void InitEditableTitleField()
        {
            _editableTitleField = NodeUtils.CreateNodeEditableTitleField("Answer Variant");
            _editableTitleField.RegisterValueChangedCallback(InvokeTitleChangedEvent);
            Add(_editableTitleField);
        }

        private void InvokeTitleChangedEvent(ChangeEvent<string> context)
        {
            TitleChanged?.Invoke(this, context);
        }

        private void InitButtonsBlock()
        {
            _buttonContainer = new VisualElement();
            Add(_buttonContainer);
            _reorderButtonsContainer = new VisualElement();
            _moveUpReorderButton = new Button(InvokeMoveUpReorderButtonClickedEvent);
            _moveDownReorderButton = new Button(InvokeMoveDownReorderButtonClickedEvent);
            _reorderButtonsContainer.Add(_moveUpReorderButton);
            _reorderButtonsContainer.Add(_moveDownReorderButton);
            _deleteButton = new Button(InvokeDeleteButtonClickedEvent);
            _buttonContainer.Add(_reorderButtonsContainer);
            _buttonContainer.Add(_deleteButton);
        }

        private void InvokeDeleteButtonClickedEvent()
        {
            DeleteButtonClicked?.Invoke(this);
        }

        private void InvokeMoveUpReorderButtonClickedEvent()
        {
            MoveUpReorderButtonClicked?.Invoke(this);
        }

        private void InvokeMoveDownReorderButtonClickedEvent()
        {
            MoveDownReorderButtonClicked?.Invoke(this);
        }

        private void InitStyleClasses()
        {
            AddToClassList(AnswerVariantPortUssClassName);
            _editableTitleField.AddToClassList(AnswerVariantPortEditableTitleFieldUssClassName);
            _buttonContainer.AddToClassList(AnswerVariantPortButtonContainerUssClassName);
            _deleteButton.AddToClassList(AnswerVariantPortDeleteButtonUssClassName);
            _reorderButtonsContainer.AddToClassList(ReorderButtonsContainerUssClassName);
            _moveUpReorderButton.AddToClassList(MoveUpReorderButtonUssClassName);
            _moveUpReorderButton.AddToClassList(ReorderButtonUssClassName);
            _moveDownReorderButton.AddToClassList(MoveDownReorderButtonUssClassName);
            _moveDownReorderButton.AddToClassList(ReorderButtonUssClassName);
        }
    }
}
