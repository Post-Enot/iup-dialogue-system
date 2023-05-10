using System;
using System.Collections.Generic;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public abstract class BaseDialogueNode
    {
        public abstract IEnumerable<BaseDialogueEdge> Edges { get; }
        public abstract Vector2 PositionOnGraph { get; set; }
    }
}
