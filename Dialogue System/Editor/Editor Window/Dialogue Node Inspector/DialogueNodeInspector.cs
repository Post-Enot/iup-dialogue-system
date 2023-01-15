using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class DialogueNodeInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DialogueNodeInspector, UxmlTraits> { }

        public sealed class SerializedDialogueNode : ScriptableObject
        {
            public LocalizedString LocalizedString => _localizedString;

            [SerializeField] private LocalizedString _localizedString;
            private SerializedObject _serializedObject;

            public SerializedProperty GetLocalizedStringSerializedProperty()
            {
                LazyInitSerializedObject();
                return _serializedObject.FindProperty(nameof(_localizedString));
            }

            private void LazyInitSerializedObject()
            {
                _serializedObject ??= new SerializedObject(this);
            }
        }

        public DialogueNodeInspector()
        {
            InitClasses();
            InitNodeNameField();
            InitActorField();
            InitIsStartNodeField();
            InitLocalizedStringField();
        }

        private const string _visualElementClassInspectorField = "iup-dialogue-tree__inspector-field";

        private SerializedDialogueNode _serializedDialogueNode;

        private void InitClasses()
        {
            AddToClassList("iup-dialogue-tree-element-inspector");
            AddToClassList("iup-dialogue-node-inspector");
        }

        private void InitNodeNameField()
        {
            var field = new TextField()
            {
                label = "Node Name"
            };
            field.AddToClassList(_visualElementClassInspectorField);
            Add(field);
        }

        private void InitActorField()
        {
            var field = new DropdownField()
            {
                label = "Actor",
                choices = new List<string>()
                {
                    "None",
                    "Main Hero",
                    "Voice-From-The-Void"
                }
            };
            field.AddToClassList(_visualElementClassInspectorField);
            Add(field);
        }

        private void InitIsStartNodeField()
        {
            var field = new Toggle()
            {
                label = "Is Start Node"
            };
            field.AddToClassList(_visualElementClassInspectorField);
            Add(field);
        }

        private void InitLocalizedStringField()
        {
            _serializedDialogueNode = ScriptableObject.CreateInstance<SerializedDialogueNode>();
            var field = new PropertyField();
            SerializedProperty localizedStringSerializedProperty =
                _serializedDialogueNode.GetLocalizedStringSerializedProperty();
            field.BindProperty(localizedStringSerializedProperty);
            field.AddToClassList(_visualElementClassInspectorField);
            Add(field);
        }
    }
}
