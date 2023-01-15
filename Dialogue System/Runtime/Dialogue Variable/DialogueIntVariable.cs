using System;

namespace IUP.Toolkits.DialogueSystem
{
    public sealed class DialogueIntVariable : DialogueVariable<int>
    {
        public override bool Equals(object obj)
        {
            return (obj is DialogueIntVariable variable) && (Value == variable.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator <(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value < b.Value;
        }

        public static bool operator <=(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator >(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value > b.Value;
        }

        public static bool operator >=(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator ==(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(DialogueIntVariable a, DialogueIntVariable b)
        {
            return a.Value != b.Value;
        }
    }
}
