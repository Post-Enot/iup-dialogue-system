using System;
using System.Collections.Generic;
using IUP.Toolkits.SerializableCollections;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class DialogueTreeBuilder
    {
        [field: SerializeReference] public EntryNodeBuilder EntryNode { get; }
        [field: SerializeReference] public ExitNodeBuilder ExitNode { get; }
        public IEnumerable<BaseDialogueNodeBuilder> Nodes => _nodes.Value;

        [SerializeField] private SRHashSet<BaseDialogueNodeBuilder> _nodes;

        public SpeechNodeBuilder CreateSpeechNode(string nodeName, Vector2 positionOnGraph)
        {
            SpeechNodeBuilder speechNode = new(nodeName, positionOnGraph);
            _ = _nodes.Value.Add(speechNode);
            return speechNode;
        }

        public AnswerNodeBuilder CreateAnswerNode(string nodeName, Vector2 positionOnGraph)
        {
            AnswerNodeBuilder answerNode = new(nodeName, positionOnGraph);
            _ = _nodes.Value.Add(answerNode);
            return answerNode;
        }

        //public void RemoveNode(BaseDialogueNode node);

        //public void ConnectNodes(OutputPortBuilder outputPort, InputPortBuilder inputPort);

        //public void ReconnectNodes(DialogueEdgeBuilder edge, OutputPortBuilder outputPort);

        //public void ReconnectNodes(DialogueEdgeBuilder edge, InputPortBuilder inputPort);

        //public void DisconnectNodes(DialogueEdgeBuilder edge);
    }
}
