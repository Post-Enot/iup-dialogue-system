namespace IUP.Toolkits.DialogueSystem
{
    public sealed class TransitCondition<TVariable, T> : ITransitCondition
        where TVariable : DialogueVariable<T>
    {
        public TransitCondition(
            TVariable a,
            TVariable b,
            DialogueVariable<T>.Condition<TVariable> condition)
        {
            A = a;
            B = b;
            Condition = condition;
        }

        public TVariable A { get; }
        public TVariable B { get; }
        public DialogueVariable<T>.Condition<TVariable> Condition { get; }

        public bool IsTrue()
        {
            return Condition.IsTrue(A, B);
        }
    }
}
