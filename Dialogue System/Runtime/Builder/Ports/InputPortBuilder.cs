using System;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class InputPortBuilder : IInputPortBuilder
    {
        public BaseDialogueNodeBuilder InputNode => throw new NotImplementedException();
        public BaseDialogueEdgeBuilder Edge => throw new NotImplementedException();
    }
}
