using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class ExitNode : BaseDialogueNode
    {
        public ExitNode() : base()
        {
            title = "Exit";
            titleButtonContainer.Clear();

            MakeInputOnly();
            MakeUndeletable();
            InitInputPort();
            InitStyleClasses();
        }

        public ExitNode(Vector2 position) : this()
        {
            Rect positionRect = new(position, Vector2.zero);
            SetPositionWithoutNotify(positionRect);
        }

        public static readonly string ExitNodeUssClassName = "iup-exit-node";

        public InputDialoguePort InputPort { get; private set; }

        private void InitInputPort()
        {
            InputPort = NodeUtils.CreateInputDialoguePort();
            inputContainer.Add(InputPort);
        }

        private void MakeInputOnly()
        {
            topContainer.Remove(outputContainer);
            topContainer.Remove(PortContainersDivider);
        }

        private void MakeUndeletable()
        {
            capabilities &= ~Capabilities.Deletable;
        }

        private void InitStyleClasses()
        {
            AddToClassList(ExitNodeUssClassName);
        }
    }
}
