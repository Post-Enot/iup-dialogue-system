using System;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class ActorBuilder
    {
        public ActorBuilder(string actorName)
        {
            ActorName = actorName;
        }

        public string ActorName { get; set; }
    }
}
