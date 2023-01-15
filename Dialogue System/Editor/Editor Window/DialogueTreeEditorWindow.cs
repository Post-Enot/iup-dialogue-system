using UnityEditor;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    internal sealed class DialogueTreeEditorWindow : EditorWindow
    {
        private DialogueTreeView _dialogueTreeView;
        private VisualElement _inspectorPane;
        private DialogueNodeInspector _dialogueNodeInspector;

        private void Awake()
        {
            titleContent.text = "Dialogue Tree Editor";
        }

        private void CreateGUI()
        {
            InitRootVisualElement();
            InitInspectorPane();
            InitDialogueTreeView();
        }

        private void DialogueTreeView_NodeUnselected(DialogueNode obj)
        {
            _inspectorPane.Remove(_dialogueNodeInspector);
        }

        private void DialogueTreeView_NodeSelected(DialogueNode obj)
        {
            _inspectorPane.Add(_dialogueNodeInspector);
        }

        private void InitRootVisualElement()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                UI_FilePathes.UXML_DialogueTreeEditorWindow);
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UI_FilePathes.USS_DialogueTreeEditorWindow);
            root.styleSheets.Add(styleSheet);
            visualTree.CloneTree(root);
        }

        private void InitDialogueTreeView()
        {
            _dialogueTreeView = rootVisualElement.Q<DialogueTreeView>("dialogue-tree-view");
            _dialogueTreeView.NodeSelected += DialogueTreeView_NodeSelected;
            _dialogueTreeView.NodeUnselected += DialogueTreeView_NodeUnselected;
        }

        private void InitInspectorPane()
        {
            _inspectorPane = rootVisualElement.Q<VisualElement>("inspector-pane");
            _dialogueNodeInspector = new DialogueNodeInspector();
        }

        [MenuItem("IUP/Dialogue Tree Editor")]
        public static void OpenWindow()
        {
            _ = GetWindow<DialogueTreeEditorWindow>();
        }
    }
}
