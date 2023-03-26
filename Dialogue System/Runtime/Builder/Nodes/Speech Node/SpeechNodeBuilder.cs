using System;
using UnityEngine;
using UnityEngine.Localization;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class SpeechNodeBuilder : BaseDialogueNodeBuilder
    {
        public SpeechNodeBuilder(string nodeName, Vector2 positionOnGraph)
        {
            NodeName = nodeName;
            PositionOnGraph = positionOnGraph;
        }

        public Vector2 PositionOnGraph { get; set; }
        public string NodeName { get; set; }
        public ActorBuilder Actor { get; set; }
        public LocalizedString LocalizedString { get; set; }
    }
}
