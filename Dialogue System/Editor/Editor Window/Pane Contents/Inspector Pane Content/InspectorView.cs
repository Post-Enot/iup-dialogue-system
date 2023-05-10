namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class InspectorView : PaneContentView
    {
        public InspectorView()
        {
            InitStyleClasses();
        }

        public static readonly string InspectorViewUssClassName = "iup-inspector-view";

        public static SpeechStatementBlock CreateSpeechStatementBlock()
        {
            SpeechStatementBlock speechStatementBlock = new();
            speechStatementBlock.Foldout.AddToClassList(PaneContentViewFoldoutUssClassName);
            return speechStatementBlock;
        }

        private void InitStyleClasses()
        {
            AddToClassList(InspectorViewUssClassName);
        }
    }
}
