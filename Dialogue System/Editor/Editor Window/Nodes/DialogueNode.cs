using System;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class DialogueNode : BaseDialogueTreeNode
    {
        public new class UxmlFactory : UxmlFactory<DialogueNode, UxmlTraits> { }

        public DialogueNode() : base()
        {
            InstantiateNameField();
            InitInputPort();
            InitOutputPort();
            style.width = 150;
            style.height = 75;
        }

        public event Action<DialogueNode> Selected;
        public event Action<DialogueNode> Unselected;

        public override void OnSelected()
        {
            Selected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            Unselected?.Invoke(this);
        }

        private void InstantiateNameField()
        {
            var field = new TextField()
            {
                label = "",
                value = "Speach"
            };
            field.AddToClassList("iup-dialogue-node__name-field");
            titleContainer.Insert(0, field);
        }

        private void InitInputPort()
        {
            InputDialoguePort inputPort = InstantiateInputDialoguePort();
            inputContainer.Add(inputPort);
        }

        private void InitOutputPort()
        {
            OutputDialoguePort outputPort = InstantiateOutputDialoguePort();
            outputContainer.Add(outputPort);
        }
    }
}
