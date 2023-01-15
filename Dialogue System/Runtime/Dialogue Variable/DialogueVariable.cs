namespace IUP.Toolkits.DialogueSystem
{
    public abstract class DialogueVariable<T>
    {
        public abstract class Condition<T1> where T1 : DialogueVariable<T>
        {
            public abstract bool IsTrue(T1 a, T1 b);
        }

        public T Value { get; set; }
    }
}
