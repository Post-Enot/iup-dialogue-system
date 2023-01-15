using System.Collections.Generic;

namespace IUP.Toolkits.DialogueSystem
{
    public sealed class DialogueBranch
    {
        public DialogueNode PreviousDialogue { get; private set; }
        public DialogueNode NextDialogue { get; private set; }
        public bool CanTransit
        {
            get
            {
                foreach (ITransitCondition condition in TransitConditions)
                {
                    if (!condition.IsTrue())
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public IReadOnlyCollection<ITransitCondition> TransitConditions { get; private set; }
    }
}
