using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public abstract class BaseDialogueTreeNode : Node
    {
        public const int MovingStep = 15;

        public BaseDialogueTreeNode() : base()
        {
            InitStyleClasses();
        }

        public override void SetPosition(Rect position)
        {
            style.position = Position.Absolute;
            style.left = (int)position.x - ((int)position.x % MovingStep);
            style.top = (int)position.y - ((int)position.y % MovingStep);
        }

        private void InitStyleClasses()
        {
            titleContainer.AddToClassList("iup-base-dialogue-tree-node__title-container");
            inputContainer.AddToClassList("iup-base-dialogue-tree-node__input-container");
            outputContainer.AddToClassList("iup-base-dialogue-tree-node__output-container");
            extensionContainer.AddToClassList("iup-base-dialogue-tree-node__extension-container");
            mainContainer.AddToClassList("iup-base-dialogue-tree-node__main-container");
        }

        public InputDialoguePort InstantiateInputDialoguePort()
        {
            return new InputDialoguePort(Orientation.Horizontal, Port.Capacity.Multi)
            {
                portName = ""
            };
        }

        public OutputDialoguePort InstantiateOutputDialoguePort()
        {
            return new OutputDialoguePort(Orientation.Horizontal, Port.Capacity.Multi)
            {
                portName = ""
            };
        }
    }
}
