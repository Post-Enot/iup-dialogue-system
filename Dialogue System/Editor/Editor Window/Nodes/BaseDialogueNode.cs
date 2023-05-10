using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public abstract class BaseDialogueNode : Node
    {
        public BaseDialogueNode() : base(DialogueNodeUxmlFilePath)
        {
            PortContainersDivider = topContainer.Q<VisualElement>("divider");
            InitStyleClasses();
        }

        public static readonly string DialogueNodeUxmlFilePath = "Assets/Dialogue System UI/DialogueNode.uxml";
        public static readonly string DialogueNodeUssClassName = "iup-dialogue-node";

        public VisualElement PortContainersDivider { get; }

        public event Action<BaseDialogueNode> PositionChanged;

        public override Rect GetPosition()
        {
            return new Rect(
                style.left.value.value,
                style.top.value.value,
                layout.width,
                layout.height);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            PositionChanged?.Invoke(this);
        }

        public void SetPositionWithoutNotify(Rect newPosition)
        {
            base.SetPosition(newPosition);
        }

        private void InitStyleClasses()
        {
            AddToClassList(DialogueNodeUssClassName);
        }
    }
}
