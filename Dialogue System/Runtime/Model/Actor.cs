using System;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public sealed class Actor
    {
        public Actor(string name, Color color)
        {
            ActorName = name;
            ActorColor = color;
        }

        [field: SerializeField] public string ActorName { get; set; }
        [field: SerializeField] public Color ActorColor { get; set; }
    }
}
