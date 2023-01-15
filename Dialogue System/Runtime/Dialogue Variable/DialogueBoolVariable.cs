using System;

namespace IUP.Toolkits.DialogueSystem
{
    public class DialogueBoolVariable : DialogueVariable<bool>
    {
        public sealed class IsEqualCondition : Condition<DialogueBoolVariable>
        {
            public override bool IsTrue(DialogueBoolVariable a, DialogueBoolVariable b)
            {
                return a == b;
            }
        }

        public sealed class IsNotEqualCondition : Condition<DialogueBoolVariable>
        {
            public override bool IsTrue(DialogueBoolVariable a, DialogueBoolVariable b)
            {
                return a != b;
            }
        }

        public DialogueBoolVariable() { }

        public DialogueBoolVariable(bool variable)
        {
            Value = variable;
        }

        public IsEqualCondition IsEqual
        {
            get
            {
                _isEqual ??= new IsEqualCondition();
                return _isEqual;
            }
        }
        public IsNotEqualCondition IsNotEqual
        {
            get
            {
                _isNotEqual ??= new IsNotEqualCondition();
                return _isNotEqual;
            }
        }

        private static IsEqualCondition _isEqual;
        private static IsNotEqualCondition _isNotEqual;

        public override bool Equals(object obj)
        {
            return (obj is DialogueBoolVariable variable) && (Value == variable.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator !(DialogueBoolVariable variable)
        {
            return !variable.Value;
        }

        public static bool operator true(DialogueBoolVariable variable)
        {
            return variable.Value;
        }

        public static bool operator false(DialogueBoolVariable variable)
        {
            return variable.Value;
        }

        public static bool operator ==(DialogueBoolVariable a, DialogueBoolVariable b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(DialogueBoolVariable a, DialogueBoolVariable b)
        {
            return a.Value != b.Value;
        }
    }
}
