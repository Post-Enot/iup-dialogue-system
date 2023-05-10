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
            InitBackground();
            InitManipulators();
            InitSystemNodes();

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UI_FilePathes.USS_DialogueTreeEditorWindow);
            styleSheets.Add(styleSheet);

            graphViewChanged += HandleGraphViewChange;
        }

        public EntryNode EntryNode { get; private set; }
        public ExitNode ExitNode { get; private set; }
        public GridBackground GridBackground { get; private set; }

        private readonly HashSet<Edge> _removedEdgesInLastChange = new();

        public event Action<BaseDialogueNode> NodeCreatedByUser;
        public event Action<BaseDialogueNode> NodeDeletedByUser;
        public event Action<BaseDialogueNode> NodePositionChanged;
        public event Action<Edge> EdgeCreated;
        public event Action<Edge> EdgeDeleted;
        public event Action<Edge> EdgeReconnected;
        public event Action<GraphElement> GraphElementSelected;
        public event Action<GraphElement> GraphElementUnselected;
        public event Action SelectionClear;

        public override void AddToSelection(ISelectable selectable)
        {
            if (selectable is GraphElement graphElement)
            {
                GraphElementSelected?.Invoke(graphElement);
            }
            base.AddToSelection(selectable);
        }

        public override void ClearSelection()
        {
            SelectionClear?.Invoke();
            base.ClearSelection();
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            if (selectable is GraphElement graphElement)
            {
                GraphElementUnselected?.Invoke(graphElement);
            }
            base.RemoveFromSelection(selectable);
        }

        public override EventPropagation DeleteSelection()
        {
            foreach (ISelectable selectable in selection)
            {
                if ((selectable is not GraphElement graphElement) ||
                    (!graphElement.capabilities.HasFlag(Capabilities.Deletable)))
                {
                    continue;
                }

                switch (selectable)
                {
                    case BaseDialogueNode dialogueTreeNode:
                        NodeDeletedByUser?.Invoke(dialogueTreeNode);
                        break;

                    case Edge edge:
                        EdgeDeleted?.Invoke(edge);
                        break;
                }
            }
            return base.DeleteSelection();
        }

        private GraphViewChange HandleGraphViewChange(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge createdEdge in graphViewChange.edgesToCreate)
                {
                    if (_removedEdgesInLastChange.Contains(createdEdge))
                    {
                        _ = _removedEdgesInLastChange.Remove(createdEdge);
                        EdgeReconnected?.Invoke(createdEdge);
                    }
                    else
                    {
                        EdgeCreated?.Invoke(createdEdge);
                    }
                }
            }

            _removedEdgesInLastChange.Clear();
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement removedGraphElement in graphViewChange.elementsToRemove)
                {
                    switch (removedGraphElement)
                    {
                        case Edge removedEdge:
                            _ = _removedEdgesInLastChange.Add(removedEdge);
                            break;
                    }
                }
            }
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            (endPort.direction != startPort.direction) &&
            (endPort.node != startPort.node)).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(
                "Add Speech Node",
                (DropdownMenuAction context) =>
                {
                    Vector2 nodePosition = GetLocalMousePosition(context.eventInfo.mousePosition);
                    SpeechNode speechNode = CreateSpeechNode(nodePosition, "Speech Node");
                    NodeCreatedByUser?.Invoke(speechNode);
                });
            evt.menu.AppendAction(
                "Add Answer Node",
                (DropdownMenuAction context) =>
                {
                    Vector2 nodePosition = GetLocalMousePosition(context.eventInfo.mousePosition);
                    AnswerNode answerNode = CreateAnswerNode(nodePosition, "Answer Node");
                    NodeCreatedByUser?.Invoke(answerNode);
                });
        }

        public SpeechNode CreateSpeechNode(Vector2 position, string title)
        {
            SpeechNode speechNode = new(position, title);
            AddElement(speechNode);
            speechNode.PositionChanged += OnNodePositionChanged;
            return speechNode;
        }

        public AnswerNode CreateAnswerNode(Vector2 position, string title)
        {
            AnswerNode answerNode = new(position, title);
            AddElement(answerNode);
            answerNode.PositionChanged += OnNodePositionChanged;
            answerNode.AnswerPortRemoved += DeleteEdgesConnectedWithDeletedPort;
            return answerNode;
        }

        private void DeleteEdgesConnectedWithDeletedPort(
            AnswerNode answerNode,
            int answerVariantPortIndex,
            AnswerVariantPort deletedAnswerVariantPort)
        {
            foreach (Edge edge in deletedAnswerVariantPort.connections)
            {
                edge.input.Disconnect(edge);
                RemoveElement(edge);
            }
            deletedAnswerVariantPort.DisconnectAll();
        }

        private ExitNode CreateExitNode(Vector2 position)
        {
            ExitNode exitNode = new(position);
            AddElement(exitNode);
            exitNode.PositionChanged += OnNodePositionChanged;
            return exitNode;
        }

        private EntryNode CreateEntryNode(Vector2 position)
        {
            EntryNode entryNode = new(position);
            AddElement(entryNode);
            entryNode.PositionChanged += OnNodePositionChanged;
            return entryNode;
        }


        private void InitSystemNodes()
        {
            EntryNode = CreateEntryNode(default);
            ExitNode = CreateExitNode(default);
        }

        private void InitManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private void InitBackground()
        {
            GridBackground gridBackground = new();
            Insert(0, gridBackground);
        }

        private void OnNodePositionChanged(BaseDialogueNode node)
        {
            NodePositionChanged?.Invoke(node);
        }

        private Vector2 GetLocalMousePosition(Vector2 mousePosition)
        {
            return contentViewContainer.WorldToLocal(mousePosition);
        }
    }
}
