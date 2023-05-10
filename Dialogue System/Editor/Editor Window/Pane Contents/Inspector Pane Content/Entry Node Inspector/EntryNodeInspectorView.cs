using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class EntryNodeInspectorView : InspectorView
    {
        public new class UxmlFactory : UxmlFactory<EntryNodeInspectorView, UxmlTraits> { }

        public EntryNodeInspectorView()
        {
            Add(new Label("Entry node inspector"));
        }
    }
}
