using System;

namespace IUP.Toolkits.DialogueSystem
{
    public sealed class DialogueStringVariable : DialogueVariable<string>
    {
        public override bool Equals(object obj)
        {
            return (obj is DialogueStringVariable variable) && (Value == variable.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(DialogueStringVariable a, DialogueStringVariable b)
        {
            return a.Value== b.Value;
        }

        public static bool operator !=(DialogueStringVariable a, DialogueStringVariable b)
        {
            return a.Value != b.Value;
        }
    }
}
