using System;
using System.Collections.Generic;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class EntryNode : BaseDialogueNode
    {
        public EntryNode()
        {
            OutputPort = new OutputPort(this);
        }

        [field: SerializeReference] public OutputPort OutputPort { get; private set; }
        [field: SerializeField] public override Vector2 PositionOnGraph { get; set; }

        public override IEnumerable<BaseDialogueEdge> Edges => OutputPort.Edges;
    }
}
