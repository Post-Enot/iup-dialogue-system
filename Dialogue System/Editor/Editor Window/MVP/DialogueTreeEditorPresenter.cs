using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueNodeModel = IUP.Toolkits.DialogueSystem.BaseDialogueNode;
using DialogueNodeView = IUP.Toolkits.DialogueSystem.Editor.BaseDialogueNode;

using EntryNodeModel = IUP.Toolkits.DialogueSystem.EntryNode;
using EntryNodeView = IUP.Toolkits.DialogueSystem.Editor.EntryNode;

using ExitNodeModel = IUP.Toolkits.DialogueSystem.ExitNode;
using ExitNodeView = IUP.Toolkits.DialogueSystem.Editor.ExitNode;

using SpeechNodeModel = IUP.Toolkits.DialogueSystem.SpeechNode;
using SpeechNodeView = IUP.Toolkits.DialogueSystem.Editor.SpeechNode;

using AnswerNodeModel = IUP.Toolkits.DialogueSystem.AnswerNode;
using AnswerNodeView = IUP.Toolkits.DialogueSystem.Editor.AnswerNode;

using AnswerPortModel = IUP.Toolkits.DialogueSystem.AnswerPort;
using AnswerPortView = IUP.Toolkits.DialogueSystem.Editor.AnswerVariantPort;

using EdgeModel = IUP.Toolkits.DialogueSystem.DialogueEdge;
using EdgeView = UnityEditor.Experimental.GraphView.Edge;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    [Serializable]
    public sealed class DialogueTreeEditorPresenter
    {
        public DialogueTreeEditorPresenter()
        {
            InitEditableTreeForEmptyAsset();
            _inspectorPresenter = new InspectorPresenter();
        }

        public DialogueTreeEditorView EditorView { get; private set; }
        [field: SerializeField] public DialogueTreeAsset EditableAsset { get; private set; }
        [field: SerializeField] public DialogueTree EditableDialogueTree { get; private set; } = new();

        public static readonly Vector2 DefaultEntryNodePosition = new();
        public static readonly Vector2 DefaultExitNodePosition = new(800, 0);
        public static readonly string DefaultActorName = "Noname";
        public static readonly string DefaultAnswerVariantPortTitle = "Answer Variant";
        public static readonly Color DefaultActorColor = Color.gray;

        private readonly BijectiveDictionary<DialogueNodeModel, DialogueNodeView> _nodeViewModelSet = new();
        private readonly BijectiveDictionary<EdgeModel, EdgeView> _edgeViewModelSet = new();
        private readonly InspectorPresenter _inspectorPresenter = new();

        public event Action<string> UserHasTakenActionStart;
        public event Action UserHasTakenActionEnd;

        public void SetEditorView(VisualElement editorWindowRootVisualElement)
        {
            UnsubscribeFromViewChanges();
            EditorView = new DialogueTreeEditorView(editorWindowRootVisualElement);
            _inspectorPresenter.SetView(EditorView.InspectorPaneContentView);
            SynchViewWithModel();
            SubscribeToViewChanges();
        }

        public void SetEditableAsset(DialogueTreeAsset editableAsset)
        {
            EditableAsset = editableAsset;
            if (EditableAsset != null)
            {
                EditableDialogueTree = new DialogueTree(EditableAsset.DialogueTree);
            }
            else
            {
                InitEditableTreeForEmptyAsset();
            }
            _inspectorPresenter.SetEditableDialogueTree(EditableDialogueTree);
            SynchViewWithModel();
        }

        public void SaveChangesToAsset()
        {
            EditableAsset.DialogueTree = new DialogueTree(EditableDialogueTree);
        }

        public void SynchViewWithModel()
        {
            if (EditorView != null)
            {
                EditorView.SetEditableAssetFieldValueWithoutNotify(EditableAsset);
                SynchActorsList();
                SynchNodes();
                SynchEdges();
            }
        }

        public void Repaint() // TODO
        {
            //foreach (DialogueNodeModel node in EditableDialogueTree.Nodes)
            //{
            //    switch (node)
            //    {
            //        case ExitNodeModel exitNodeModel:
            //            ExitNodeView exitNodeView = _nodeViewModelSet.ValueByKey[exitNodeModel] as ExitNodeView;
            //            exitNodeView.SetPositionWithoutNotify(new Rect(exitNodeModel.PositionOnGraph, Vector2.zero));
            //            break;
            //    }
            //}
        }

        #region Actors methods
        private void SynchActorsList()
        {
            EditorView.ActorsListView.BindItem += BindActorListItem;
            EditorView.ActorsListView.UnbindItem += UnbindActorListItem;
            EditorView.ActorsListView.SetSize(EditableDialogueTree.Actors.Count);

            EditorView.ActorsListView.ItemsAdded += HandleActorsAddedEvent;
            EditorView.ActorsListView.ItemsRemoved += HandleActorsRemovedEvent;
            EditorView.ActorsListView.ItemsIndexChanged += HandleActorIndexChangedEvent;
        }

        private void HandleActorsAddedEvent(IEnumerable<int> addedActorsIndexes)
        {
            UserHasTakenActionStart?.Invoke("Added actors.");
            foreach (int addedActorIndex in addedActorsIndexes)
            {
                _ = EditableDialogueTree.AddActorAt(addedActorIndex, DefaultActorName, DefaultActorColor);
            }
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleActorsRemovedEvent(IEnumerable<int> removedActorIndexes)
        {
            UserHasTakenActionStart?.Invoke("Removed actors.");
            foreach (int removedActorIndex in removedActorIndexes)
            {
                EditableDialogueTree.RemoveActorByIndex(removedActorIndex);
            }
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleActorIndexChangedEvent(int from, int to)
        {
            UserHasTakenActionStart?.Invoke("Actor index changed.");
            EditableDialogueTree.ChangeActorPosition(from, to);
            UserHasTakenActionEnd?.Invoke();
        }

        private void BindActorListItem(ActorListElementView actorView, int index)
        {
            Actor actorModel = EditableDialogueTree.Actors[index];
            actorView.SetActorNameWithoutNotify(actorModel.ActorName);
            actorView.SetActorColorWithoutNotify(actorModel.ActorColor);
            actorView.ActorNameChanged += HandleActorNameChangedByUser;
            actorView.ActorColorChanged += HandleActorColorChangedByUser;
        }

        private void UnbindActorListItem(ActorListElementView actorElementListView, int index)
        {
            actorElementListView.ActorColorChanged -= HandleActorColorChangedByUser;
            actorElementListView.ActorNameChanged -= HandleActorNameChangedByUser;
        }

        private void HandleActorColorChangedByUser(
            ActorListElementView actorListElementView,
            ChangeEvent<Color> context)
        {
            UserHasTakenActionStart?.Invoke("Actor color changed.");
            int actorIndex = EditorView.ActorsListView.GetIndexOfElementView(actorListElementView);
            EditableDialogueTree.Actors[actorIndex].ActorColor = context.newValue;
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleActorNameChangedByUser(
            ActorListElementView actorListElementView,
            ChangeEvent<string> context)
        {
            UserHasTakenActionStart?.Invoke("Actor name changed.");
            int actorIndex = EditorView.ActorsListView.GetIndexOfElementView(actorListElementView);
            EditableDialogueTree.Actors[actorIndex].ActorName = context.newValue;
            UserHasTakenActionEnd?.Invoke();
        }
        #endregion

        private void SynchNodes()
        {
            foreach (DialogueNodeModel node in EditableDialogueTree.Nodes)
            {
                switch (node)
                {
                    case EntryNodeModel entryNodeModel:
                        Rect positionRect = new(entryNodeModel.PositionOnGraph, Vector2.zero);
                        EditorView.DialogueTreeView.EntryNode.SetPositionWithoutNotify(positionRect);
                        _nodeViewModelSet.Add(entryNodeModel, EditorView.DialogueTreeView.EntryNode);
                        break;

                    case SpeechNodeModel speechNodeModel:
                        SpeechNodeView speechNodeView = EditorView.DialogueTreeView.CreateSpeechNode(
                            speechNodeModel.PositionOnGraph,
                            speechNodeModel.Title);
                        speechNodeView.TitleChanged += SpeechNodeView_TitleChanged;
                        _nodeViewModelSet.Add(speechNodeModel, speechNodeView);
                        break;

                    case AnswerNodeModel answerNodeModel:
                        AnswerNodeView answerNodeView = EditorView.DialogueTreeView.CreateAnswerNode(
                            answerNodeModel.PositionOnGraph,
                            answerNodeModel.Title);
                        foreach (AnswerPortModel answerPortModel in answerNodeModel.AnswerPorts)
                        {
                            answerNodeView.AddAnswerPort(answerPortModel.Title);
                        }
                        answerNodeView.AnswerPortAdded += HandleAddingNewAnswerPortOnAnswerNodeView;
                        answerNodeView.AnswerPortRemoved += HandleRemovingAnswerPortOnAnswerNodeView;
                        answerNodeView.AnswerPortPositionChanged += HandleAnswerVariantPortPositionChangedEvent;
                        answerNodeView.TitleChanged += AnswerNodeView_TitleChanged;
                        answerNodeView.AnswerPortTitleChanged += AnswerNodeView_AnswerPortTitleChanged;
                        _nodeViewModelSet.Add(answerNodeModel, answerNodeView);
                        break;

                    case ExitNodeModel exitNodeModel:
                        positionRect = new(exitNodeModel.PositionOnGraph, Vector2.zero);
                        EditorView.DialogueTreeView.ExitNode.SetPositionWithoutNotify(positionRect);
                        _nodeViewModelSet.Add(exitNodeModel, EditorView.DialogueTreeView.ExitNode);
                        break;

                    default:
                        throw new NotImplementedException(); // TODO.
                }
            }
        }

        private InputDialoguePort GetInputDialoguePortViewByEdgeModel(EdgeModel edgeModel)
        {
            DialogueNodeView dialogueNodeView = _nodeViewModelSet.ValueByKey[edgeModel.InputNode];
            return dialogueNodeView switch
            {
                ExitNodeView exitNodeView => exitNodeView.InputPort,
                SpeechNodeView speechNodeView => speechNodeView.InputPort,
                AnswerNodeView answerNodeView => answerNodeView.InputPort,
                _ => throw new NotImplementedException() // TODO.
            };
        }

        private OutputDialoguePort GetOutputDialoguePortViewByEdgeModel(EdgeModel edgeModel)
        {
            DialogueNodeView dialogueNodeView = _nodeViewModelSet.ValueByKey[edgeModel.OutputNode];
            switch (dialogueNodeView)
            {
                case EntryNodeView entryNodeView:
                    return entryNodeView.OutputPort;

                case SpeechNodeView speechNodeView:
                    return speechNodeView.OutputPort;

                case AnswerNodeView answerNodeView:
                    var answerNodeModel = edgeModel.OutputNode as AnswerNodeModel;
                    var answerVariantPortModel = edgeModel.OutputPort as AnswerPortModel;
                    int answerVariantPortIndex = answerNodeModel.AnswerPorts.IndexOf(answerVariantPortModel);
                    return answerNodeView.AnswerPorts[answerVariantPortIndex];

                default:
                    throw new NotImplementedException(); // TODO.
            }
        }

        private void SynchEdges()
        {
            foreach (EdgeModel edgeModel in EditableDialogueTree.Edges.Cast<EdgeModel>())
            {
                InputDialoguePort inputDialoguePortView = GetInputDialoguePortViewByEdgeModel(edgeModel);
                OutputDialoguePort outputDialoguePortView = GetOutputDialoguePortViewByEdgeModel(edgeModel);
                EdgeView edgeView = inputDialoguePortView.ConnectTo(outputDialoguePortView);
                _edgeViewModelSet.Add(edgeModel, edgeView);
                EditorView.DialogueTreeView.AddElement(edgeView);
            }
        }

        private void SpeechNodeView_TitleChanged(
            SpeechNodeView speechNodeView,
            ChangeEvent<string> context)
        {
            UserHasTakenActionStart?.Invoke("Speech node title changed.");
            var speechNodeModel = _nodeViewModelSet.KeyByValue[speechNodeView] as SpeechNodeModel;
            speechNodeModel.Title = context.newValue;
            UserHasTakenActionEnd?.Invoke();
        }

        private void AnswerNodeView_TitleChanged(
            AnswerNodeView answerNodeView,
            ChangeEvent<string> context)
        {
            UserHasTakenActionStart?.Invoke("Answer node title changed.");
            var answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
            answerNodeModel.Title = context.newValue;
            UserHasTakenActionEnd?.Invoke();
        }

        #region Answer Variant Ports
        private void AnswerNodeView_AnswerPortTitleChanged(
            AnswerNodeView answerNodeView,
            int answerVariantPortIndex,
            ChangeEvent<string> context)
        {
            UserHasTakenActionStart?.Invoke("Answer variant port title changed.");
            AnswerNodeModel answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
            answerNodeModel.AnswerPorts[answerVariantPortIndex].Title = context.newValue;
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleAddingNewAnswerPortOnAnswerNodeView(
            AnswerNodeView answerNodeView,
            int portIndex)
        {
            AnswerNodeModel answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
            answerNodeView.AnswerPorts[portIndex].SetTitleWithoutNotify(DefaultAnswerVariantPortTitle);
            UserHasTakenActionStart?.Invoke("Answer variant port added.");
            _ = answerNodeModel.AddAnswerPort(answerNodeView.AnswerPorts[portIndex].Title);
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleRemovingAnswerPortOnAnswerNodeView(
            AnswerNodeView answerNodeView,
            int portIndex,
            AnswerPortView answerPortView)
        {
            AnswerNodeModel answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
            AnswerPortModel answerPortModel = answerNodeModel.AnswerPorts[portIndex];
            UserHasTakenActionStart?.Invoke("Answer variant port removed.");
            answerNodeModel.RemoveAnswerPortByIndex(portIndex);
            List<BaseDialogueEdge> edgesModelsToDelete = new(answerPortModel.Edges);
            foreach (EdgeModel edgeModel in edgesModelsToDelete.Cast<EdgeModel>())
            {
                EdgeView edgeView = _edgeViewModelSet.ValueByKey[edgeModel];
                EditableDialogueTree.DeleteEdge(edgeModel);
                _edgeViewModelSet.Remove(edgeModel, edgeView);
            }
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleAnswerVariantPortPositionChangedEvent(
            AnswerNodeView answerNodeView,
            int portMovedFrom,
            int portMovedTo)
        {
            AnswerNodeModel answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
            UserHasTakenActionStart?.Invoke("Answer variant port position changed.");
            answerNodeModel.ChangeAnswerVariantPortPosition(portMovedFrom, portMovedTo);
            UserHasTakenActionEnd?.Invoke();
        }
        #endregion

        private void InitEditableTreeForEmptyAsset()
        {
            EditableDialogueTree = new DialogueTree();
            EditableDialogueTree.EntryNode.PositionOnGraph = DefaultEntryNodePosition;
            EditableDialogueTree.ExitNode.PositionOnGraph = DefaultExitNodePosition;
        }

        private void SubscribeToViewChanges()
        {
            if (EditorView != null)
            {
                EditorView.DialogueTreeView.NodeCreatedByUser += HandleNodeCreatedEvent;
                EditorView.DialogueTreeView.NodeDeletedByUser += HandleNodeDeletedEvent;
                EditorView.DialogueTreeView.NodePositionChanged += HandleNodePositionChangedEvent;
                EditorView.DialogueTreeView.EdgeCreated += HandleEdgeCreatedEvent;
                EditorView.DialogueTreeView.EdgeDeleted += HandleEdgeDeletedEvent;
                EditorView.DialogueTreeView.EdgeReconnected += HandleEdgeReconnectedEvent;
                EditorView.DialogueTreeView.GraphElementSelected += HandleGraphElementSelectedEvent;
                EditorView.DialogueTreeView.GraphElementUnselected += HandleGraphElementUnselectedEvent;
                EditorView.DialogueTreeView.SelectionClear += HandleSelectionClearEvent;
            }
        }

        private void HandleSelectionClearEvent()
        {
            _inspectorPresenter.CloseOpenedInspector();
        }

        private void HandleGraphElementSelectedEvent(GraphElement graphElement)
        {
            switch (graphElement)
            {
                case EntryNodeView entryNodeView:
                    var entryNodeModel = _nodeViewModelSet.KeyByValue[entryNodeView] as EntryNodeModel;
                    _inspectorPresenter.OpenEntryNodeInspector(entryNodeModel);
                    break;

                case SpeechNodeView speechNodeView:
                    var speechNodeModel = _nodeViewModelSet.KeyByValue[speechNodeView] as SpeechNodeModel;
                    _inspectorPresenter.OpenSpeechNodeInspector(speechNodeModel);
                    break;

                case AnswerNodeView answerNodeView:
                    var answerNodeModel = _nodeViewModelSet.KeyByValue[answerNodeView] as AnswerNodeModel;
                    _inspectorPresenter.OpenAnswerNodeInspector(answerNodeModel);
                    break;

                case ExitNodeView exitNodeView:
                    var exitNodeModel = _nodeViewModelSet.KeyByValue[exitNodeView] as ExitNodeModel;
                    _inspectorPresenter.OpenExitNodeInspector(exitNodeModel);
                    break;

                case EdgeView edgeView:
                    EdgeModel edgeModel = _edgeViewModelSet.KeyByValue[edgeView];
                    _inspectorPresenter.OpenEdgeInspector(edgeModel);
                    break;
            }
        }

        private void HandleGraphElementUnselectedEvent(GraphElement graphElement)
        {
            _inspectorPresenter.CloseOpenedInspector();
        }

        private void UnsubscribeFromViewChanges()
        {
            if (EditorView != null)
            {
                EditorView.DialogueTreeView.NodeCreatedByUser -= HandleNodeCreatedEvent;
                EditorView.DialogueTreeView.NodeDeletedByUser -= HandleNodeDeletedEvent;
                EditorView.DialogueTreeView.NodePositionChanged -= HandleNodePositionChangedEvent;
                EditorView.DialogueTreeView.EdgeCreated -= HandleEdgeCreatedEvent;
                EditorView.DialogueTreeView.EdgeDeleted -= HandleEdgeDeletedEvent;
            }
        }

        private void HandleNodeCreatedEvent(DialogueNodeView createdNodeView)
        {
            Rect positionRect = createdNodeView.GetPosition();
            switch (createdNodeView)
            {
                case SpeechNodeView speechNodeView:
                    UserHasTakenActionStart?.Invoke("Speech Node Added.");
                    SpeechNodeModel speechNodeModel = EditableDialogueTree.CreateSpeechNode(
                        speechNodeView.Title,
                        positionRect.position);
                    speechNodeView.TitleChanged += SpeechNodeView_TitleChanged;
                    _nodeViewModelSet.Add(speechNodeModel, speechNodeView);
                    UserHasTakenActionEnd?.Invoke();
                    break;

                case AnswerNodeView answerNodeView:
                    UserHasTakenActionStart?.Invoke("Answer Node Added.");
                    AnswerNodeModel answerNodeModel = EditableDialogueTree.CreateAnswerNode(
                        answerNodeView.Title,
                        positionRect.position);
                    answerNodeView.AnswerPortAdded += HandleAddingNewAnswerPortOnAnswerNodeView;
                    answerNodeView.AnswerPortRemoved += HandleRemovingAnswerPortOnAnswerNodeView;
                    answerNodeView.AnswerPortPositionChanged += HandleAnswerVariantPortPositionChangedEvent;
                    answerNodeView.TitleChanged += AnswerNodeView_TitleChanged;
                    answerNodeView.AnswerPortTitleChanged += AnswerNodeView_AnswerPortTitleChanged;
                    _nodeViewModelSet.Add(answerNodeModel, answerNodeView);
                    UserHasTakenActionEnd?.Invoke();
                    break;

                default:
                    throw new NotImplementedException(); // TODO
            }
        }

        private void HandleNodeDeletedEvent(DialogueNodeView removedNodeView)
        {
            UserHasTakenActionStart?.Invoke("Node Removed.");
            DialogueNodeModel nodeModel = _nodeViewModelSet.KeyByValue[removedNodeView];
            _ = _nodeViewModelSet.Remove(nodeModel, removedNodeView);
            EditableDialogueTree.DeleteNode(nodeModel);
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleNodePositionChangedEvent(DialogueNodeView movedNodeView)
        {
            DialogueNodeModel nodeModel = _nodeViewModelSet.KeyByValue[movedNodeView];
            UserHasTakenActionStart?.Invoke("Node Position Changed.");
            nodeModel.PositionOnGraph = movedNodeView.GetPosition().position;
            UserHasTakenActionEnd?.Invoke();
        }

        #region Edge view events
        private IInputPort GetInputPortByNodeView(EdgeView createdEdge)
        {
            var inputNodeView = createdEdge.input.node as DialogueNodeView;
            DialogueNodeModel inputNodeModel = _nodeViewModelSet.KeyByValue[inputNodeView];
            return inputNodeModel switch
            {
                ExitNodeModel exitNodeModel => exitNodeModel.InputPort,
                SpeechNodeModel speechNodeModel => speechNodeModel.InputPort,
                AnswerNodeModel answerNodeModel => answerNodeModel.InputPort,
                _ => throw new NotImplementedException() // TODO.
            };
        }

        private IOutputPort GetOutputPortByNodeView(EdgeView createdEdgeView)
        {
            var outputNodeView = createdEdgeView.output.node as DialogueNodeView;
            DialogueNodeModel outputNodeModel = _nodeViewModelSet.KeyByValue[outputNodeView];
            switch (outputNodeModel)
            {
                case EntryNodeModel entryNodeModel:
                    return entryNodeModel.OutputPort;

                case SpeechNodeModel speechNodeModel:
                    return speechNodeModel.OutputPort;

                case AnswerNodeModel answerNodeModel:
                    var answerNodeView = outputNodeView as AnswerNodeView;
                    var answerPortView = createdEdgeView.output as AnswerPortView;
                    int portIndex = answerNodeView.AnswerPorts.IndexOf(answerPortView);
                    return answerNodeModel.AnswerPorts[portIndex];

                default:
                    throw new NotImplementedException(); // TODO.
            }
        }

        private void HandleEdgeCreatedEvent(EdgeView createdEdgeView) // TODO.
        {
            UserHasTakenActionStart?.Invoke("Dialogue Edge Created.");
            var inputNodeView = createdEdgeView.input.node as DialogueNodeView;
            var outputNodeView = createdEdgeView.output.node as DialogueNodeView;
            DialogueNodeModel inputNodeModel = _nodeViewModelSet.KeyByValue[inputNodeView];
            DialogueNodeModel outputNodeModel = _nodeViewModelSet.KeyByValue[outputNodeView];

            IInputPort inputPort = GetInputPortByNodeView(createdEdgeView);
            IOutputPort outputPort = GetOutputPortByNodeView(createdEdgeView);

            EdgeModel createdEdgeModel = EditableDialogueTree.CreateEdgeBetweenPorts(inputPort, outputPort);
            _edgeViewModelSet.Add(createdEdgeModel, createdEdgeView);

            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleEdgeDeletedEvent(EdgeView deletedEdgeView)
        {
            UserHasTakenActionStart?.Invoke("Dialogue Edge Deleted.");
            EdgeModel deletedEdgeModel = _edgeViewModelSet.KeyByValue[deletedEdgeView];
            EditableDialogueTree.DeleteEdge(deletedEdgeModel);
            UserHasTakenActionEnd?.Invoke();
        }

        private void HandleEdgeReconnectedEvent(EdgeView reconnectedEdge) // TODO.
        {
            Debug.Log("Edge was reconnected.");
        }
        #endregion
    }
}
