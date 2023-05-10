using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class EdgeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<EdgeInspectorView, UxmlTraits> { }

        public EdgeInspectorView()
        {
            Add(new Label("Edge inspector"));
        }
    }
}
