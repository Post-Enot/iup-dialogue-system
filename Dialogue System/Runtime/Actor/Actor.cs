using System;

namespace IUP.Toolkits.DialogueSystem
{
    public sealed class Actor
    {
        public Actor(string actorName)
        {
            RenameActor(actorName);
        }

        public string ActorName { get; private set; }

        public void RenameActor(string actorName)
        {
            ActorName = actorName ?? throw new NullReferenceException(nameof(actorName));
        }
    }
}
