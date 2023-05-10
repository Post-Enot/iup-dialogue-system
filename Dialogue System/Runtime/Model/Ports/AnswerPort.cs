using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class AnswerPort : BaseDialoguePort, IOutputPort
    {
        public AnswerPort(AnswerNode parentNode, string title)
        {
            ParentNode = parentNode;
            Title = title;
        }

        [field: SerializeReference] public BaseDialogueNode ParentNode { get; private set; }
        public IEnumerable<BaseDialogueEdge> Edges => _dialogueEdges.Value;

        [field: SerializeField] public string Title { get; set; }
        [field: SerializeReference] public Actor Actor { get; set; }
        [field: SerializeField] public LocalizedString PreviewLocalizedString { get; set; }
        [field: SerializeField] public LocalizedAudioClip PreviewLocalizedAudioClip { get; set; }
        [field: SerializeField] public LocalizedString AnswerLocalizedString { get; set; }
        [field: SerializeField] public LocalizedAudioClip AnswerLocalizedAudioClip { get; set; }

        [SerializeField] private EdgesSRHashSet _dialogueEdges = new();

        public void ConnectEdge(BaseDialogueEdge edge)
        {
            bool isAdded = _dialogueEdges.Value.Add(edge);
            if (!isAdded)
            {
                throw new InvalidOperationException(ConnectEdgeExceptionMessage); // TODO
            }
        }

        public void DisconnectEdge(BaseDialogueEdge edge)
        {
            bool isRemoved = _dialogueEdges.Value.Remove(edge);
            if (!isRemoved)
            {
                throw new InvalidOperationException(DisconnectEdgeExceptionMessage); // TODO
            }
        }
    }
}
