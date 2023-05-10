using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    /// <summary>
    /// Ассет диалогового дерева.
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue Tree", menuName = "IUP/Dialogue Tree Asset")]
    public sealed class DialogueTreeAsset : ScriptableObject
    {
        /// <summary>
        /// Стандартное положение узла входа.
        /// </summary>
        public static readonly Vector2 DefaultEntryNodePosition = new(0, 0);
        /// <summary>
        /// Стандартное положение узла выхода.
        /// </summary>
        public static readonly Vector2 DefaultExitNodePosition = new(800, 0);

        /// <summary>
        /// Диалоговое дерево. Назначение и изменение предполагается только из редактора.
        /// </summary>
        [field: HideInInspector]
        [field: SerializeField]
        public DialogueTree DialogueTree { get; set; } = new(
            DefaultEntryNodePosition,
            DefaultExitNodePosition);
    }
}
