using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class OutputAnswerPort : DialoguePort
    {
        public OutputAnswerPort(Orientation orientation, Capacity capacity)
            : base(orientation, Direction.Output, capacity)
        {
            var buttonContainer = new VisualElement();
            Add(buttonContainer);
            var settingButton = new Button();
            buttonContainer.Add(settingButton);
            buttonContainer.AddToClassList("iup-output-answer-port__button-container");
            settingButton.AddToClassList("iup-output-answer-port__setting-button");
            AddToClassList("iup-output-answer-port");
        }
    }
}
