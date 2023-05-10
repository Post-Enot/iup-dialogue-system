using System.Collections.Generic;

namespace IUP.Toolkits.DialogueSystem
{
    public interface IInputPort
    {
        public BaseDialogueNode ParentNode { get; }
        public IEnumerable<BaseDialogueEdge> Edges { get; }

        public void DisconnectEdge(BaseDialogueEdge edge);

        public void ConnectEdge(BaseDialogueEdge edge);
    }
}
