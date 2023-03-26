using System;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class AnswerNodeBuilder : BaseDialogueNodeBuilder
    {
        public AnswerNodeBuilder(string nodeName, Vector2 positionOnGraph)
        {
            NodeName = nodeName;
            PositionOnGraph = positionOnGraph;
        }

        public string NodeName { get; set; }
        public Vector2 PositionOnGraph { get; set; }
    }
}
