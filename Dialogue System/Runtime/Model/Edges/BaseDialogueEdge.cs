using System;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem
{
    [Serializable]
    public abstract class BaseDialogueEdge
    {
        [field: SerializeReference] public IInputPort InputPort { get; private set; }
        public BaseDialogueNode InputNode => InputPort.ParentNode;
        [field: SerializeReference] public IOutputPort OutputPort { get; private set; }
        public BaseDialogueNode OutputNode => OutputPort.ParentNode;

        public virtual void Disconnect()
        {
            InputPort?.DisconnectEdge(this);
            OutputPort?.DisconnectEdge(this);
        }

        public virtual void ConnectWith(IInputPort inputPort)
        {
            InputPort?.DisconnectEdge(this);
            InputPort = inputPort;
            InputPort?.ConnectEdge(this);
        }

        public virtual void ConnectWith(IOutputPort outputPort)
        {
            OutputPort?.DisconnectEdge(this);
            OutputPort = outputPort;
            OutputPort?.ConnectEdge(this);
        }

        public virtual void ConnectWith(
            IInputPort inputPort,
            IOutputPort outputPort)
        {
            ConnectWith(inputPort);
            ConnectWith(outputPort);
        }
    }
}
