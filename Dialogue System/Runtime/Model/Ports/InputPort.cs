using System;
using System.Collections.Generic;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class InputPort : BaseDialoguePort, IInputPort
    {
        public InputPort(BaseDialogueNode parentNode)
        {
            ParentNode = parentNode;
        }

        [field: SerializeReference] public BaseDialogueNode ParentNode { get; private set; }
        public IEnumerable<BaseDialogueEdge> Edges => _dialogueEdges.Value;

        [SerializeField] private EdgesSRHashSet _dialogueEdges = new();

        public void ConnectEdge(BaseDialogueEdge edge)
        {
            bool isAdded = _dialogueEdges.Value.Add(edge);
            if (!isAdded)
            {
                throw new InvalidOperationException(ConnectEdgeExceptionMessage); // TODO.
            }
        }

        public void DisconnectEdge(BaseDialogueEdge edge)
        {
            bool isRemoved = _dialogueEdges.Value.Remove(edge);
            if (!isRemoved)
            {
                throw new InvalidOperationException(DisconnectEdgeExceptionMessage); // TODO.
            }
        }
    }
}
