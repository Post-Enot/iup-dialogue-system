using UnityEditor;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public abstract class PropertyFieldSerializedObject<T> : ScriptableObject
    {
        public T SerializedData
        {
            get => _serializedData;
            set => _serializedData = value;
        }
        public SerializedProperty SerializedProperty
        {
            get
            {
                LazyInitSerializedProperty();
                return _serializedProperty;
            }
        }

        [SerializeField] private T _serializedData;
        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;

        private void LazyInitSerializedProperty()
        {
            if (_serializedProperty is null)
            {
                LazyInitSerializedObject();
                _serializedProperty = _serializedObject.FindProperty(nameof(_serializedData));
            }
        }

        private void LazyInitSerializedObject()
        {
            _serializedObject ??= new SerializedObject(this);
        }
    }
}
