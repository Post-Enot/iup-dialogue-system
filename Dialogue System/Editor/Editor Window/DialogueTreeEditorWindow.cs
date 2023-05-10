using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    internal sealed class DialogueTreeEditorWindow : EditorWindow
    {
        [SerializeField] private DialogueTreeEditorPresenter _editorPresenter = new();

        private void Awake()
        {
            UpdateTitle();
            Undo.undoRedoPerformed += OnUndoRendoPerformed;
            _editorPresenter.UserHasTakenActionStart += OnUserHasTakenActionStart;
            _editorPresenter.UserHasTakenActionEnd += OnUserHasTakenActionEnd;
        }

        private void CreateGUI()
        {
            InitRootVisualElement();
            _editorPresenter.SetEditorView(rootVisualElement);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            _editorPresenter.SaveChangesToAsset();
            hasUnsavedChanges = false;
        }

        private void InitRootVisualElement()
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                UI_FilePathes.UXML_DialogueTreeEditorWindow);
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UI_FilePathes.USS_DialogueTreeEditorWindow);
            rootVisualElement.styleSheets.Add(styleSheet);
            visualTree.CloneTree(rootVisualElement);
        }

        #region Undo-rendo action methods
        private void OnUndoRendoPerformed()
        {
            _editorPresenter.Repaint();
        }

        private void OnUserHasTakenActionStart(string actionName)
        {
            Undo.RecordObject(this, actionName);
            hasUnsavedChanges = true;
        }

        private void OnUserHasTakenActionEnd()
        {
            EditorUtility.SetDirty(this);
        }
        #endregion

        #region Asset methods
        private void AssignEditableAsset(DialogueTreeAsset editableAsset)
        {
            _editorPresenter.SetEditableAsset(editableAsset);
            UpdateTitle();
        }
        #endregion

        #region Title methods
        private void UpdateTitle()
        {
            if (_editorPresenter.EditableAsset != null)
            {
                SetTitleByEditableAsset();
            }
            else
            {
                SetTitleForEditorWithoutAsset();
            }
        }

        private void SetTitleForEditorWithoutAsset()
        {
            titleContent.text = "Dialogue Tree Editor";
        }

        private void SetTitleByEditableAsset()
        {
            titleContent.text = _editorPresenter.EditableAsset.name;
        }
        #endregion

        #region Open editor static methods
        [MenuItem("IUP/Dialogue Tree Editor")]
        public static void OpenEditorOrFocusOnAlreadyOpen()
        {
            _ = GetWindow<DialogueTreeEditorWindow>();
        }

        public static DialogueTreeEditorWindow FindEditorForAsset(DialogueTreeAsset asset)
        {
            DialogueTreeEditorWindow[] windows = Resources.FindObjectsOfTypeAll<DialogueTreeEditorWindow>();
            return windows.FirstOrDefault(window => window._editorPresenter.EditableAsset == asset);
        }

        public static DialogueTreeEditorWindow OpenEditorForAsset(DialogueTreeAsset asset)
        {
            DialogueTreeEditorWindow window = FindEditorForAsset(asset);
            if (window == null)
            {
                window = CreateInstance<DialogueTreeEditorWindow>();
                window.AssignEditableAsset(asset);
            }
            window.Show();
            window.Focus();
            return window;
        }

        [OnOpenAsset(OnOpenAssetAttributeMode.Execute)]
        [SuppressMessage("Style", "IDE0060:Удалите неиспользуемый параметр", Justification = "<Ожидание>")]
        [SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "<Ожидание>")]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            Object assetObject = EditorUtility.InstanceIDToObject(instanceId);
            if (assetObject is DialogueTreeAsset dialogueTreeAsset)
            {
                _ = OpenEditorForAsset(dialogueTreeAsset);
                return true;
            }
            return false;
        }
        #endregion
    }
}
