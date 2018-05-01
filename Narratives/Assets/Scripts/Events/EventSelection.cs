using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSelection : MonoBehaviour {

    // Event initialization
    private static int numEvents = 12;
    private Event[] events = new Event[numEvents];

    // Event selection
    private int eventPickNumber = 0;
    private int oldEvent = 0;

    private bool readyForNewEvent = true;

    // Event Creation in Start()
    void Start ()
    {
        events[0] = new Event("Flood");
        events[1] = new Event("Witch");
    }

    private void Update()
    {
        if (readyForNewEvent)
        {
            SelectEvent();
            readyForNewEvent = false;
        }
    }

    public void SetReadyForNewEvent()
    {
        readyForNewEvent = true;
    }
 
    // Pick an event and execute it.
    void SelectEvent()
    {
        // Pick an event we did not have last cycle.
        while(eventPickNumber == oldEvent)
        {
            eventPickNumber = Random.Range(0, numEvents);
        }

        oldEvent = eventPickNumber; // Store the event we had this time

        // Get the name of the event, and execute it in the handler.
        string eventName = events[eventPickNumber].GetName();
        this.gameObject.GetComponent<EventHandler>().HandleEvent(eventName);
    }
}
