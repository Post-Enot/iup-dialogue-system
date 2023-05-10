using System;
using System.Collections.Generic;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class ExitNode : BaseDialogueNode
    {
        public ExitNode()
        {
            InputPort = new InputPort(this);
        }

        [field: SerializeReference] public InputPort InputPort { get; private set; }
        [field: SerializeField] public override Vector2 PositionOnGraph { get; set; }

        public override IEnumerable<BaseDialogueEdge> Edges => InputPort.Edges;
    }
}
