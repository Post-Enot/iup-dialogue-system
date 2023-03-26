namespace IUP.Toolkits.DialogueSystem.Builder
{
    public interface IInputPortBuilder
    {
        public BaseDialogueNodeBuilder InputNode { get; }
        public BaseDialogueEdgeBuilder Edge { get; }
    }
}
