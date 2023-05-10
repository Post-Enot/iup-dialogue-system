using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class AnswerNodeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<AnswerNodeInspectorView, UxmlTraits> { }

        public AnswerNodeInspectorView()
        {
            Add(new Label("answer node inspector"));
        }
    }
}
