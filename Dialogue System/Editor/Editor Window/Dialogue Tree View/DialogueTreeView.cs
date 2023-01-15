using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class DialogueTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueTreeView, UxmlTraits> { }

        public DialogueTreeView()
        {
            var gridBackground = new GridBackground();
            Insert(0, gridBackground);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UI_FilePathes.USS_DialogueTreeEditorWindow);
            CreateDefaultNodes();
            styleSheets.Add(styleSheet);
        }

        public event Action<DialogueNode> NodeSelected;
        public event Action<DialogueNode> NodeUnselected;

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            (endPort.direction != startPort.direction) &&
            (endPort.node != startPort.node)).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(
                "Create Dialogue Node",
                (context) => CreateDialogueNode(GetLocalMousePosition(context.eventInfo.mousePosition)));
            evt.menu.AppendAction(
                "Create Answer Node",
                (context) => CreateAnswerNode(GetLocalMousePosition(context.eventInfo.mousePosition)));
        }

        private void CreateDialogueNode(Vector2 position)
        {
            var dialogueNode = new DialogueNode();
            dialogueNode.Selected += OnNodeSelected;
            dialogueNode.Unselected += OnNodeUnselected;
            AddElement(dialogueNode);
            dialogueNode.SetPosition(new(position, Vector2.zero));
        }

        private void CreateAnswerNode(Vector2 position)
        {
            var answerNode = new AnswerNode();
            AddElement(answerNode);
            answerNode.SetPosition(new(position, Vector2.zero));
        }

        private void CreateDefaultNodes()
        {
            var enterNode = new EnterNode();
            AddElement(enterNode);
            enterNode.SetPosition(new(100, 500, 0, 0));
            var exitNode = new ExitNode();
            AddElement(exitNode);
            exitNode.SetPosition(new(800, 500, 0, 0));
        }

        private void OnNodeSelected(DialogueNode dialogueNode)
        {
            NodeSelected?.Invoke(dialogueNode);
        }

        private void OnNodeUnselected(DialogueNode dialogueNode)
        {
            NodeUnselected?.Invoke(dialogueNode);
        }

        private Vector2 GetLocalMousePosition(Vector2 mousePosition)
        {
            return contentViewContainer.WorldToLocal(mousePosition);
        }
    }
}
