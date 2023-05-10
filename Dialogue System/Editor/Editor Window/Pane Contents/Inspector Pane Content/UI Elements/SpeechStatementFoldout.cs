using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class SpeechStatementBlock : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SpeechStatementBlock, UxmlTraits> { }

        public SpeechStatementBlock()
        {
            InitFoldout();
            InitActorDropdownField();
            InitLocalizedStringField();
            InitLocalizedAudioClipField();

            Add(Foldout);
            Foldout.Add(ActorDropdownField);
            Foldout.Add(LocalizedStringField);
            Foldout.Add(LocalizedAudioClipField);
            InitStyleClasses();
        }

        public static readonly string PaneFoldoutBlockUssClassName = "iup-pane-foldout-block";

        public Foldout Foldout { get; private set; }
        public DropdownField ActorDropdownField { get; private set; }
        public PropertyField LocalizedStringField { get; private set; }
        public PropertyField LocalizedAudioClipField { get; private set; }

        private LocalizedStringSerializedObject _localizedStringSerializedObject;
        private LocalizedAudioClipSerializedObject _localizedAudioClipSerializedObject;

        public event Action<ChangeEvent<string>> ActorChanged;
        public event Action<LocalizedString> LocalizedStringChanged;
        public event Action<LocalizedAudioClip> LocalizedAudioClipChanged;

        public void SetLocalizedStringWithoutNotify(LocalizedString localizedString)
        {
            _localizedStringSerializedObject.SerializedData = localizedString;
            LocalizedStringField.MarkDirtyRepaint();
        }

        public void SetLocalizedAudioClipWithoutNotify(LocalizedAudioClip localizedAudioClip)
        {
            _localizedAudioClipSerializedObject.SerializedData = localizedAudioClip;
            LocalizedAudioClipField.MarkDirtyRepaint();
        }

        private void InitFoldout()
        {
            Foldout = new Foldout
            {
                text = "Speech Statement"
            };
        }

        private void InitActorDropdownField()
        {
            ActorDropdownField = new DropdownField()
            {
                label = "Actor"
            };
            ActorDropdownField.RegisterValueChangedCallback(InvokeActorChangedEvent);
        }

        private void InitLocalizedStringField()
        {
            _localizedStringSerializedObject =
                ScriptableObject.CreateInstance<LocalizedStringSerializedObject>();
            LocalizedStringField = new PropertyField()
            {
                label = "Localized String"
            };
            LocalizedStringField.BindProperty(_localizedStringSerializedObject.SerializedProperty);
            LocalizedStringField.RegisterValueChangeCallback(InvokeLocalizedStringChangedEvent);
        }

        private void InitLocalizedAudioClipField()
        {
            _localizedAudioClipSerializedObject =
                ScriptableObject.CreateInstance<LocalizedAudioClipSerializedObject>();
            LocalizedAudioClipField = new PropertyField()
            {
                label = "Localized Audio Clip"
            };
            LocalizedAudioClipField.BindProperty(_localizedAudioClipSerializedObject.SerializedProperty);
            LocalizedAudioClipField.RegisterValueChangeCallback(InvokeLocalizedAudioClipChangedEvent);
        }

        private void InvokeActorChangedEvent(ChangeEvent<string> context)
        {
            ActorChanged?.Invoke(context);
        }

        private void InvokeLocalizedStringChangedEvent(SerializedPropertyChangeEvent context)
        {
            LocalizedStringChanged?.Invoke(_localizedStringSerializedObject.SerializedData);
        }

        private void InvokeLocalizedAudioClipChangedEvent(SerializedPropertyChangeEvent context)
        {
            LocalizedAudioClipChanged?.Invoke(_localizedAudioClipSerializedObject.SerializedData);
        }

        private void InitStyleClasses()
        {
            Foldout.AddToClassList(PaneFoldoutBlockUssClassName);
        }
    }
}
