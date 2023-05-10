using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.UIElements;

namespace IUP.Toolkits.DialogueSystem.Editor
{
    public class BaseInspectorPresenter
    {
        public static readonly string NoActorValueForDropdownField = "None";

        public virtual void Repaint() { }

        public static void InitActorDropdownField(
            DropdownField dropdownField,
            ReadOnlyCollection<Actor> actors,
            Actor chosenActor)
        {
            int chosenActorIndex = chosenActor != null ? actors.IndexOf(chosenActor) + 1 : 0;
            dropdownField.choices = new List<string>()
            {
                NoActorValueForDropdownField
            };
            foreach (Actor actor in actors)
            {
                dropdownField.choices.Add(actor.ActorName);
            }
            dropdownField.index = chosenActorIndex;
        }
    }
}
