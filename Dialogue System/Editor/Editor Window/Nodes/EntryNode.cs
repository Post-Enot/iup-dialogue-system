using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class EntryNode : BaseDialogueNode
    {
        public EntryNode() : base()
        {
            title = "Entry";
            titleButtonContainer.Clear();

            MakeUndeletable();
            MakeOutputOnly();
            InitOutputPort();
            InitStyleClasses();
        }

        public EntryNode(Vector2 position) : this()
        {
            Rect positionRect = new(position, Vector2.zero);
            SetPositionWithoutNotify(positionRect);
        }

        public static readonly string EntryNodeUssClassName = "iup-entry-node";

        public OutputDialoguePort OutputPort { get; private set; }

        private void InitOutputPort()
        {
            OutputPort = NodeUtils.CreateOutputDialoguePort();
            outputContainer.Add(OutputPort);
        }

        private void MakeUndeletable()
        {
            capabilities &= ~Capabilities.Deletable;
        }

        private void MakeOutputOnly()
        {
            topContainer.Remove(inputContainer);
            topContainer.Remove(PortContainersDivider);
        }

        private void InitStyleClasses()
        {
            AddToClassList(EntryNodeUssClassName);
        }
    }
}
