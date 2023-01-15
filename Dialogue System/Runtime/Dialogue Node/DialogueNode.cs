using System.Collections.Generic;
using UnityEngine.Localization;

namespace IUP.Toolkits.DialogueSystem
{
    public sealed class DialogueNode
    {
        public Actor Actor { get; private set; }
        public LocalizedString LocalizedString { get; private set; }
        public IReadOnlyList<DialogueBranch> Branches { get; private set; }
    }
}
