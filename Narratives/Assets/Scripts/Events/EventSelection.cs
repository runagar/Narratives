using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSelection : MonoBehaviour {

    WorkloadHandler workloadHandler;
    VillageStats villageStats;

    // Event initialization
    private static int numEvents = 2;
    private MonthlyEvent[] events = new MonthlyEvent[numEvents];

    // Event selection
    private int eventPickNumber = 0;
    private int oldEvent = 0;

    private bool readyForNewEvent = true;

    // Event Creation in Start()
    void Start ()
    {
        workloadHandler = GameObject.Find("VillageStatHandler").GetComponent<WorkloadHandler>();
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
        events[0] = new MonthlyEvent("Flood", 4);
        events[1] = new MonthlyEvent("Flood", 4);
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
        int eventPickBreaker = 0;
        // Pick an event we did not have last cycle.
        while(eventPickNumber == oldEvent || !events[eventPickNumber].GetAvailable())
        {
            eventPickNumber = Random.Range(0, numEvents);
            eventPickBreaker++;
            if (eventPickBreaker > 30) break;
        }

        oldEvent = eventPickNumber; // Store the event we had this time

        foreach(MonthlyEvent e in events)
        {
            e.UpdateAvailabilityTimer();
        }
        events[eventPickNumber].SetUnavilable();

        // Get the name of the event, and execute it in the handler.
        string eventName = events[eventPickNumber].GetName();
        this.gameObject.GetComponent<EventHandler>().HandleEvent(eventName);

        workloadHandler.UpdateWorkload();
        villageStats.UpdateVillage();
    }
}
