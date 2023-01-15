using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class OutputDialoguePort : DialoguePort
    {
        public OutputDialoguePort(
            Orientation orientation,
            Capacity capacity) : base(orientation, Direction.Output, capacity) { }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            Debug.Log("Output connect");
        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            Debug.Log("Output disconnect");
        }
    }
}
