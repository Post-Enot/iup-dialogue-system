using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class ExitNodeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<ExitNodeInspectorView, UxmlTraits> { }

        public ExitNodeInspectorView()
        {
            Add(new Label("Exit node inspector"));
        }
    }
}
