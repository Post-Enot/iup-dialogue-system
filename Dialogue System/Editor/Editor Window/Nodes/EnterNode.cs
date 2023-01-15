using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class EnterNode : BaseDialogueTreeNode
    {
        public EnterNode() : base()
        {
            InitOutputPort();
            InstantiateNameLabel();
            titleContainer.style.backgroundColor = new Color(0f / 255f, 63f / 255f, 38f / 255f, 1f);
            style.width = 75;
            style.height = 75;
            titleButtonContainer.Clear();
            AddToClassList("iup-enter-node");
        }

        private void InitOutputPort()
        {
            OutputDialoguePort outputPort = InstantiateOutputDialoguePort();
            outputContainer.Add(outputPort);
        }

        private void InstantiateNameLabel()
        {
            title = "Enter";
        }
    }
}
