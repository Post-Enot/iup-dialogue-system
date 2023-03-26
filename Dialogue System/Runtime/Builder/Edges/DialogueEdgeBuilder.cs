using System;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class DialogueEdgeBuilder
    {
        public IInputPortBuilder InputPort { get; private set; }
        public BaseDialogueNodeBuilder InputNode => InputPort.InputNode;
        public IOutputPortBuilder OutputPort { get; private set; }
        public BaseDialogueNodeBuilder OutputNode => OutputPort.OutputNode;
    }
}
