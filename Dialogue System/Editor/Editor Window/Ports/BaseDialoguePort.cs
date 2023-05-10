using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public abstract class BaseDialoguePort : Port
    {
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private readonly GraphViewChange _graphViewChange;
            private readonly List<Edge> _edgesToCreate = new();
            private readonly List<GraphElement> _edgesToDelete = new();

            public DefaultEdgeConnectorListener()
            {
                _graphViewChange.edgesToCreate = _edgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position) { }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                _edgesToCreate.Clear();
                _edgesToCreate.Add(edge);

                // We can't just add these edges to delete to the _graphViewChange
                // because we want the proper deletion code in GraphView to also
                // be called. Of course, that code (in DeleteElements) also
                // sends a GraphViewChange.
                _edgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge edgeToDelete in edge.input.connections)
                    {
                        if (edgeToDelete != edge)
                        {
                            _edgesToDelete.Add(edgeToDelete);
                        }
                    }
                }
                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge edgeToDelete in edge.output.connections)
                    {
                        if (edgeToDelete != edge)
                        {
                            _edgesToDelete.Add(edgeToDelete);
                        }
                    }
                }
                if (_edgesToDelete.Count > 0)
                {
                    graphView.DeleteElements(_edgesToDelete);
                }
                var edgesToCreate = _edgesToCreate;
                if (graphView.graphViewChanged != null)
                {
                    edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
                }
                foreach (Edge edgeToCreate in edgesToCreate)
                {
                    graphView.AddElement(edgeToCreate);
                    edge.input.Connect(edgeToCreate);
                    edge.output.Connect(edgeToCreate);
                }
            }
        }

        public BaseDialoguePort(Orientation orientation, Direction direction, Capacity capacity) :
            base(orientation, direction, capacity, typeof(bool))
        {
            portName = "";
            DefaultEdgeConnectorListener connectorListener = new();
            m_EdgeConnector = new EdgeConnector<Edge>(connectorListener);
            this.AddManipulator(m_EdgeConnector);
            InitStyleClasses();
        }

        public static readonly string DialoguePortUssClassName = "iup-dialogue-port";

        public override bool ContainsPoint(Vector2 localPoint)
        {
            var rect = new Rect(0, 0, layout.width, layout.height);
            return rect.Contains(localPoint);
        }

        private void InitStyleClasses()
        {
            AddToClassList("iup-dialogue-port");
        }
    }
}
