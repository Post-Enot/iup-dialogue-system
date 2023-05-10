using UnityEditor.Experimental.GraphView;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class OutputDialoguePort : BaseDialoguePort
    {
        public OutputDialoguePort(Orientation orientation, Capacity capacity) :
            base(orientation, Direction.Output, capacity)
        {
            InitStyleClasses();
        }

        public static readonly string OutputDialoguePortUssClassName = "iup-output-dialogue-port";

        private void InitStyleClasses()
        {
            AddToClassList(OutputDialoguePortUssClassName);
        }
    }
}
