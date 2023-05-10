using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public static class NodeUtils
    {
        public static TextField CreateNodeEditableTitleField(string title = "")
        {
            TextField editableTitleField = new()
            {
                label = "",
                value = title,
                isDelayed = true
            };
            editableTitleField.AddToClassList("iup-dialogue-node__editable-title-field");
            return editableTitleField;
        }

        public static InputDialoguePort CreateInputDialoguePort()
        {
            InputDialoguePort port = new(Orientation.Horizontal, Port.Capacity.Multi)
            {
                portName = ""
            };
            port.AddToClassList("iup-dialogue-port");
            port.AddToClassList("iup-dialogue-input-port");
            return port;
        }

        public static OutputDialoguePort CreateOutputDialoguePort()
        {
            OutputDialoguePort port = new(Orientation.Horizontal, Port.Capacity.Multi)
            {
                portName = ""
            };
            port.AddToClassList("iup-dialogue-port");
            port.AddToClassList("iup-dialogue-output-port");
            return port;
        }

        public static AnswerVariantPort CreateAnswerDialoguePort()
        {
            return null;
        }
    }
}
