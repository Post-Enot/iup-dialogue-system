using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class PaneContentView : VisualElement
    {
        public PaneContentView()
        {
            InitStyleClasses();
        }

        public static readonly string PaneContentViewFoldoutUssClassName = "iup-pane-content-view__foldout";
        public static readonly string PaneContentViewUssClassName = "iup-pane-content-view";

        private void InitStyleClasses()
        {
            AddToClassList(PaneContentViewUssClassName);
        }
    }
}
