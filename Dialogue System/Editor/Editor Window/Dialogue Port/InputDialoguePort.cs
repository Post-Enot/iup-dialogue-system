using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class InputDialoguePort : DialoguePort
    {
        public InputDialoguePort(
            Orientation orientation,
            Capacity capacity) : base(orientation, Direction.Input, capacity) { }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            Debug.Log("Input connect");
        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            Debug.Log("Input disconnect");
        }
    }
}
