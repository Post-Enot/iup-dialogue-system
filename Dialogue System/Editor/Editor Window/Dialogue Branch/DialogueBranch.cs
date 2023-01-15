using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class DialogueBranch : Edge
    {
        public event Action<DialogueBranch> Selected;
        public event Action<DialogueBranch> Unselected;

        public override void Select(VisualElement selectionContainer, bool additive)
        {
            base.Select(selectionContainer, additive);
            Selected?.Invoke(this);
        }

        public override void Unselect(VisualElement selectionContainer)
        {
            base.Unselect(selectionContainer);
            Unselected?.Invoke(this);
        }
    }
}
