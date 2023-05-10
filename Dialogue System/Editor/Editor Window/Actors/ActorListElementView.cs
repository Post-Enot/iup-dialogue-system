using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class ActorListElementView : VisualElement
    {
        public ActorListElementView()
        {
            _nameField = new TextField()
            {
                isDelayed = true
            };
            _nameField.RegisterValueChangedCallback(InvokeActorNameChangedEvent);
            Add(_nameField);

            _colorField = new ColorField()
            {
                showAlpha = false
            };
            _colorField.RegisterValueChangedCallback(HandleActorColorChangedEvent);
            Add(_colorField);

            InitStyleClasses();
        }

        public ActorListElementView(string actorName, Color actorColor) : this()
        {
            ActorName = actorName;
            ActorColor = actorColor;
        }

        public static readonly string ActorListElementViewUssClassName = "iup-actor-list-element-view";
        public static readonly string ActorNameFieldUssClassName = "iup-actor-list-element-view__actor-name-field";
        public static readonly string ActorColorFieldUssClassName = "iup-actor-list-element-view__actor-color-field";

        public string ActorName
        {
            get => _nameField.value;
            set => _nameField.value = value;
        }
        public Color ActorColor
        {
            get => _colorField.value;
            set
            {
                _colorField.value = value;
                _nameField.style.color = value;
            }
        }

        private readonly TextField _nameField;
        private readonly ColorField _colorField;

        public event Action<ActorListElementView, ChangeEvent<string>> ActorNameChanged;
        public event Action<ActorListElementView, ChangeEvent<Color>> ActorColorChanged;

        public void SetActorNameWithoutNotify(string actorName)
        {
            _nameField.SetValueWithoutNotify(actorName);
        }

        public void SetActorColorWithoutNotify(Color actorColor)
        {
            _colorField.SetValueWithoutNotify(actorColor);
        }

        private void InvokeActorNameChangedEvent(ChangeEvent<string> context)
        {
            ActorNameChanged?.Invoke(this, context);
        }

        private void HandleActorColorChangedEvent(ChangeEvent<Color> context)
        {
            _nameField.style.color = context.newValue;
            ActorColorChanged?.Invoke(this, context);
        }

        private void InitStyleClasses()
        {
            AddToClassList(ActorListElementViewUssClassName);
            _nameField.AddToClassList(ActorNameFieldUssClassName);
            _colorField.AddToClassList(ActorColorFieldUssClassName);
        }
    }
}
