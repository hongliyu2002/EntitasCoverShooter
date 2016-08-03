using RMC.Common.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RMC.Common.Utilities
{
    /// <summary>
    /// Help with new UI Events
    /// </summary>
    public class EventSystemUtility
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods
        public static void AddEventTrigger(EventTrigger eventTrigger, UnityAction unityAction, EventTriggerType eventTriggerType)
        {
            EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
            triggerEvent.AddListener((eventData) => unityAction()); // ignore event data

            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = triggerEvent, eventID = eventTriggerType };

            eventTrigger.triggers.Add(entry);
        }

        public static void RemoveAllEventTriggers(EventTrigger eventTrigger)
        {
            eventTrigger.triggers.RemoveAll(e => true);
        }


    }


}
