using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public sealed class ActorsListPresenter
    {
        public ActorsListPresenter(ListView actorsListView)
        {
            _actorsListView = actorsListView;
            _actorsListView.itemsSource = _elementsList;
            _actorsListView.bindItem = HandleBindItemCallback;
            _actorsListView.unbindItem = HandleUnbindItemCallback;
            _actorsListView.makeItem = MakeItem;
            _actorsListView.itemsAdded += InvokeItemsAddedEvent;
            _actorsListView.itemsRemoved += InvokeItemsRemovedEvent;
            _actorsListView.itemIndexChanged += InvokeItemsIndexChangedEvent;
        }

        private readonly ListView _actorsListView;
        private List<ActorListElementView> _elementsList = new();

        public event Action<IEnumerable<int>> ItemsAdded;
        public event Action<IEnumerable<int>> ItemsRemoved;
        public event Action<int, int> ItemsIndexChanged;
        public event Action<ActorListElementView, int> BindItem;
        public event Action<ActorListElementView, int> UnbindItem;

        public ActorListElementView this[int index] => _elementsList[index];

        public int GetIndexOfElementView(ActorListElementView actorListElementView)
        {
            return _elementsList.FindIndex((match) => match == actorListElementView);
        }

        public void SetSize(int elementsCount)
        {
            _elementsList = new List<ActorListElementView>(elementsCount);
            for (int i = 0; i < elementsCount; i += 1)
            {
                _elementsList.Add(null);
            }
            _actorsListView.itemsSource = _elementsList;
            _actorsListView.Rebuild();
        }

        private void InvokeItemsAddedEvent(IEnumerable<int> addedItemsIndexes)
        {
            ItemsAdded?.Invoke(addedItemsIndexes);
        }

        private void InvokeItemsRemovedEvent(IEnumerable<int> removedItemsIndexes)
        {
            ItemsRemoved?.Invoke(removedItemsIndexes);
        }

        private void InvokeItemsIndexChangedEvent(int oldIndex, int newIndex)
        {
            ItemsIndexChanged?.Invoke(oldIndex, newIndex);
        }

        private void HandleBindItemCallback(VisualElement VisualElement, int index)
        {
            var actorListElementView = VisualElement as ActorListElementView;
            _actorsListView.itemsSource[index] = actorListElementView;
            BindItem?.Invoke(actorListElementView, index);
        }

        private void HandleUnbindItemCallback(VisualElement VisualElement, int index)
        {
            var actorListElementView = VisualElement as ActorListElementView;
            UnbindItem?.Invoke(actorListElementView, index);
        }

        private static ActorListElementView MakeItem()
        {
            return new ActorListElementView();
        }
    }
}
