using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IUP.Toolkits.SerializableCollections;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class DialogueTree
    {
        [Serializable] private class DynamicNodesSRHashSet : SRHashSet<BaseDialogueNode> { }

        public DialogueTree()
        {
            EntryNode = new();
            ExitNode = new();
            Actors = new ReadOnlyCollection<Actor>(_statemantActors);
        }

        public DialogueTree(Vector2 entryNodePosition, Vector2 exitNodePosition)
        {
            EntryNode = new()
            {
                PositionOnGraph = entryNodePosition
            };
            ExitNode = new()
            {
                PositionOnGraph = exitNodePosition
            };
            Actors = new ReadOnlyCollection<Actor>(_statemantActors);
        }

        public DialogueTree(DialogueTree dialogueTree) : this()
        {
            // TODO: придумать адекватное название.
            Dictionary<Actor, Actor> copyCurrentActors = new(dialogueTree.Actors.Count);
            foreach (Actor copiedActor in dialogueTree.Actors)
            {
                Actor currentActor = AddActor(copiedActor.ActorName, copiedActor.ActorColor);
                copyCurrentActors.Add(copiedActor, currentActor);
            }

            // TODO: придумать адекватное название.
            Dictionary<BaseDialogueNode, BaseDialogueNode> copyCurrentNodes = new();
            foreach (BaseDialogueNode copiedNode in dialogueTree.Nodes)
            {
                switch (copiedNode)
                {
                    case EntryNode copiedEntryNode:
                        EntryNode.PositionOnGraph = copiedEntryNode.PositionOnGraph;
                        copyCurrentNodes.Add(copiedEntryNode, EntryNode);
                        break;

                    case ExitNode copiedExitNode:
                        ExitNode.PositionOnGraph = copiedExitNode.PositionOnGraph;
                        copyCurrentNodes.Add(copiedExitNode, ExitNode);
                        break;

                    case SpeechNode copiedSpeechNode:
                        SpeechNode speechNode = CreateSpeechNode(
                            copiedSpeechNode.Title,
                            copiedSpeechNode.PositionOnGraph);
                        if (copiedSpeechNode.Actor != null)
                        {
                            speechNode.Actor = copyCurrentActors[copiedSpeechNode.Actor];
                        }
                        speechNode.LocalizedString = copiedSpeechNode.LocalizedString;
                        speechNode.LocalizedAudioClip = copiedSpeechNode.LocalizedAudioClip;
                        copyCurrentNodes.Add(copiedSpeechNode, speechNode);
                        break;

                    case AnswerNode copiedAnswerNode:
                        AnswerNode answerNode = CreateAnswerNode(
                            copiedAnswerNode.Title,
                            copiedAnswerNode.PositionOnGraph);
                        foreach (AnswerPort copiedPort in copiedAnswerNode.AnswerPorts)
                        {
                            AnswerPort answerPort = answerNode.AddAnswerPort(copiedPort.Title);
                            if (copiedPort.Actor != null)
                            {
                                answerPort.Actor = copyCurrentActors[copiedPort.Actor];
                            }
                            answerPort.AnswerLocalizedString = copiedPort.AnswerLocalizedString;
                            answerPort.AnswerLocalizedAudioClip = copiedPort.AnswerLocalizedAudioClip;
                            answerPort.PreviewLocalizedString = copiedPort.PreviewLocalizedString;
                            answerPort.PreviewLocalizedAudioClip = copiedPort.PreviewLocalizedAudioClip;
                        }
                        copyCurrentNodes.Add(copiedAnswerNode, answerNode);
                        break;

                    default:
                        throw new NotImplementedException(); // TODO.
                }
            }

            IInputPort GetInputPortByCopiedEdge(BaseDialogueEdge copiedEdge)
            {
                switch (copiedEdge.InputNode)
                {
                    case ExitNode _:
                        return ExitNode.InputPort;

                    case SpeechNode copiedSpeechNode:
                        var speechNode = copyCurrentNodes[copiedSpeechNode] as SpeechNode;
                        return speechNode.InputPort;

                    case AnswerNode copiedAnswerNode:
                        var answerNode = copyCurrentNodes[copiedAnswerNode] as AnswerNode;
                        return answerNode.InputPort;

                    default:
                        throw new NotImplementedException(); // TODO.
                }
            }

            IOutputPort GetOutputPortByCopiedEdge(BaseDialogueEdge copiedEdge)
            {
                switch (copiedEdge.OutputNode)
                {
                    case EntryNode _:
                        return EntryNode.OutputPort;

                    case SpeechNode copiedSpeechNode:
                        var speechNode = copyCurrentNodes[copiedSpeechNode] as SpeechNode;
                        return speechNode.OutputPort;

                    case AnswerNode copiedAnswerNode:
                        var answerNode = copyCurrentNodes[copiedAnswerNode] as AnswerNode;
                        AnswerPort copiedAnswerVariantPort = copiedEdge.OutputPort as AnswerPort;
                        int answerVariantPortIndex = copiedAnswerNode.AnswerPorts.IndexOf(copiedAnswerVariantPort);
                        return answerNode.AnswerPorts[answerVariantPortIndex];

                    default:
                        throw new NotImplementedException(); // TODO.
                }
            }

            foreach (BaseDialogueEdge copiedEdge in dialogueTree.Edges)
            {
                IInputPort inputPort = GetInputPortByCopiedEdge(copiedEdge);
                IOutputPort outputPort = GetOutputPortByCopiedEdge(copiedEdge);
                CreateEdgeBetweenPorts(inputPort, outputPort);
            }
        }

        [field: SerializeReference] public EntryNode EntryNode { get; private set; }
        [field: SerializeReference] public ExitNode ExitNode { get; private set; }
        public IEnumerable<BaseDialogueNode> Nodes
        {
            get
            {
                yield return EntryNode;
                foreach (BaseDialogueNode node in _dynamicNodes.Value)
                {
                    yield return node;
                }
                yield return ExitNode;
            }
        }
        public IEnumerable<BaseDialogueEdge> Edges => _dialogueEdges.Value;
        public ReadOnlyCollection<Actor> Actors { get; }

        [SerializeField] private DynamicNodesSRHashSet _dynamicNodes = new();
        [SerializeField] private EdgesSRHashSet _dialogueEdges = new();
        [SerializeReference] private List<Actor> _statemantActors = new();

        #region Actors methods
        public Actor AddActor(string actorName, Color actorColor)
        {
            Actor addedActor = new(actorName, actorColor);
            _statemantActors.Add(addedActor);
            return addedActor;
        }

        public Actor AddActorAt(int actorIndex, string actorName, Color actorColor)
        {
            Actor addedActor = new(actorName, actorColor);
            _statemantActors.Insert(actorIndex, addedActor);
            return addedActor;
        }

        /// <summary>
        /// По переданному индексу извлекает элемент из коллекции и вставляет его в переданную позицию.
        /// </summary>
        /// <param name="from">Индекс извлечения.</param>
        /// <param name="to">Индекс вставки.</param>
        public void ChangeActorPosition(int from, int to)
        {
            Actor actor = _statemantActors[from];
            _statemantActors.RemoveAt(from);
            _statemantActors.Insert(to, actor);
        }

        public void RemoveActorByIndex(int actorIndex)
        {
            Actor removedActor = _statemantActors[actorIndex];
            _statemantActors.RemoveAt(actorIndex);
            ClearNodesFromRemovedActor(removedActor);
        }

        private void ClearNodesFromRemovedActor(Actor removedActor)
        {
            foreach (BaseDialogueNode baseDialogueNode in Nodes)
            {
                switch (baseDialogueNode)
                {
                    case SpeechNode cleanupSpeechNode:
                        ClearSpeechNodeFromRemovedActor(cleanupSpeechNode, removedActor);
                        break;

                    case AnswerNode cleanupAnswerNode:
                        ClearAnswerNodeFromRemovedActor(cleanupAnswerNode, removedActor);
                        break;
                }
            }
        }

        private void ClearSpeechNodeFromRemovedActor(SpeechNode cleanupSpeechNode, Actor removedActor)
        {
            if (cleanupSpeechNode.Actor == removedActor)
            {
                cleanupSpeechNode.Actor = null;
            }
        }

        private void ClearAnswerNodeFromRemovedActor(AnswerNode cleanupAnswerNode, Actor removedActor)
        {
            foreach (AnswerPort answerPort in cleanupAnswerNode.AnswerPorts)
            {
                if (answerPort.Actor == removedActor)
                {
                    answerPort.Actor = null;
                }
            }
        }
        #endregion

        public SpeechNode CreateSpeechNode(string nodeTitle, Vector2 positionOnGraph)
        {
            SpeechNode speechNode = new(nodeTitle, positionOnGraph);
            _ = _dynamicNodes.Value.Add(speechNode);
            return speechNode;
        }

        public AnswerNode CreateAnswerNode(string nodeTitle, Vector2 positionOnGraph)
        {
            AnswerNode answerNode = new(nodeTitle, positionOnGraph);
            _ = _dynamicNodes.Value.Add(answerNode);
            return answerNode;
        }

        public void DeleteNode(BaseDialogueNode node)
        {
            foreach (BaseDialogueEdge edge in node.Edges)
            {
                edge.Disconnect();
            }
            _ = _dynamicNodes.Value.Remove(node); // TODO вызывать ошибку при невозможности удалить узел.
        }

        public DialogueEdge CreateEdgeBetweenPorts(IInputPort inputPort, IOutputPort outputPort)
        {
            DialogueEdge edge = new();
            edge.ConnectWith(inputPort, outputPort);
            _ = _dialogueEdges.Value.Add(edge);
            return edge;
        }

        public void ReconnectEdge(DialogueEdge edge, IInputPort inputPort, IOutputPort outputPort)
        {
            edge.ConnectWith(inputPort, outputPort); // TODO проверять заранее, имеется ли узел в дереве.
        }

        public void DeleteEdge(BaseDialogueEdge edge)
        {
            edge.Disconnect();
            _ = _dialogueEdges.Value.Remove(edge); // TODO проверять заранее, имеется ли узел в дереве.
        }
    }
}
