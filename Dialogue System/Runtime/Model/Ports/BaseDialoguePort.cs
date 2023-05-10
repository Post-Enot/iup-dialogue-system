using System;
using IUP.Toolkits.SerializableCollections;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public abstract class BaseDialoguePort
    {
        //[Serializable] protected sealed class EdgesSRHashSet : SRHashSet<BaseDialogueEdge> { }

        protected const string ConnectEdgeExceptionMessage =
            "Ошибка связывания порта с узлом: порт уже связан с переданным узлом.";
        protected const string DisconnectEdgeExceptionMessage =
            "Ошибка разрыва связи порта с узлом: порт не связан с переданным узлом.";
    }
}
