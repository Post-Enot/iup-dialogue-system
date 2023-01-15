using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class AnswerNode : BaseDialogueTreeNode
    {
        public AnswerNode() : base()
        {
            InitInputPort();
            InitOutputPort();
            InstantiateNameField();
            style.width = 150;
            style.height = 75;
            titleContainer.style.backgroundColor = new Color(42f / 255f, 65f / 255f, 97f / 255f, 1f);
        }

        private void InstantiateNameField()
        {
            var field = new TextField()
            {
                label = "",
                value = "Possible Answers"
            };
            field.AddToClassList("iup-dialogue-node__name-field");
            inputContainer.AddToClassList("iup-answer-node__input-container");
            outputContainer.AddToClassList("iup-answer-node__output-container");
            titleContainer.Insert(0, field);
        }

        private void InitInputPort()
        {
            InputDialoguePort inputPort = InstantiateInputDialoguePort();
            inputContainer.Add(inputPort);
        }

        private void InitOutputPort()
        {
            //OutputDialoguePort outputPort = InstantiateOutputDialoguePort();
            var outputPort = new OutputAnswerPort(Orientation.Horizontal, Port.Capacity.Single)
            {
                portName = ""
            };
            outputContainer.Add(outputPort);
        }
    }
}
