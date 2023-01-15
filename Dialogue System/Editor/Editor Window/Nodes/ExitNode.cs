using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class ExitNode : BaseDialogueTreeNode
    {
        public ExitNode() : base()
        {
            InitInputPort();
            InstantiateNameLabel();
            titleContainer.style.backgroundColor = new Color(143f / 255f, 25f / 255f, 25f / 255f, 1f);
            titleButtonContainer.Clear();
            style.width = 75;
            style.height = 75;
            AddToClassList("iup-exit-node");
        }

        private void InitInputPort()
        {
            InputDialoguePort inputPort = InstantiateInputDialoguePort();
            inputContainer.Add(inputPort);
        }

        private void InstantiateNameLabel()
        {
            title = "Exit";
        }
    }
}
