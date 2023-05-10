using UnityEditor.Experimental.GraphView;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class InputDialoguePort : BaseDialoguePort
    {
        public InputDialoguePort(Orientation orientation, Capacity capacity) :
            base(orientation, Direction.Input, capacity)
        {
            InitStyleClasses();
        }

        public static readonly string InputDialoguePortUssClassName = "iup-input-dialogue-port";

        private void InitStyleClasses()
        {
            AddToClassList(InputDialoguePortUssClassName);
        }
    }
}
