namespace IUP.Toolkits.DialogueSystem.Builder
{
    public interface IOutputPortBuilder
    {
        public BaseDialogueNodeBuilder OutputNode { get; }
        public BaseDialogueEdgeBuilder Edge { get; }
    }
}
