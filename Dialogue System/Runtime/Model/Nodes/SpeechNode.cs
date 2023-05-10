using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class SpeechNode : BaseDialogueNode
    {
        public SpeechNode(string nodeTitle, Vector2 positionOnGraph)
        {
            Title = nodeTitle;
            PositionOnGraph = positionOnGraph;
            InputPort = new InputPort(this);
            OutputPort = new OutputPort(this);
        }

        [field: SerializeField] public override Vector2 PositionOnGraph { get; set; }
        [field: SerializeField] public string Title { get; set; }
        [field: SerializeField] public LocalizedString LocalizedString { get; set; }
        [field: SerializeField] public LocalizedAudioClip LocalizedAudioClip { get; set; }
        [field: SerializeReference] public Actor Actor { get; set; }

        [field: SerializeReference] public InputPort InputPort { get; private set; }
        [field: SerializeReference] public OutputPort OutputPort { get; private set; }

        public override IEnumerable<BaseDialogueEdge> Edges
        {
            get
            {
                foreach (BaseDialogueEdge edge in InputPort.Edges)
                {
                    yield return edge;
                }
                foreach (BaseDialogueEdge edge in OutputPort.Edges)
                {
                    yield return edge;
                }
            }
        }
    }
}
