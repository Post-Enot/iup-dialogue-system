using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class AnswerNode : BaseDialogueNode
    {
        public AnswerNode(string title, Vector2 positionOnGraph)
        {
            Title = title;
            PositionOnGraph = positionOnGraph;
            InputPort = new InputPort(this);
        }

        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public override Vector2 PositionOnGraph { get; set; }
        public ReadOnlyCollection<AnswerPort> AnswerPorts
        {
            get
            {
                _readonlyAnswerPortsCollection ??= new(_answerPorts);
                return _readonlyAnswerPortsCollection;
            }
        }
        public override IEnumerable<BaseDialogueEdge> Edges
        {
            get
            {
                foreach (BaseDialogueEdge edge in InputPort.Edges)
                {
                    yield return edge;
                }
                foreach (AnswerPort answerPort in AnswerPorts)
                {
                    foreach (BaseDialogueEdge edge in answerPort.Edges)
                    {
                        yield return edge;
                    }
                }
            }
        }
        public InputPort InputPort { get; private set; }

        [SerializeReference] private List<AnswerPort> _answerPorts = new();
        private ReadOnlyCollection<AnswerPort> _readonlyAnswerPortsCollection;

        public AnswerPort AddAnswerPort(string title)
        {
            AnswerPort answerPort = new(this, title);
            _answerPorts.Add(answerPort);
            return answerPort;
        }

        public void RemoveAnswerPortByIndex(int index)
        {
            _answerPorts.RemoveAt(index); // TODO: добавить отлов ошибок и выброс более конкретной ошибки.
        }

        public void ChangeAnswerVariantPortPosition(int from, int to)
        {
            _answerPorts.MoveItemFromTo(from, to); // TODO: добавить отлов ошибок и выброс более конкретной ошибки.
        }
    }
}
