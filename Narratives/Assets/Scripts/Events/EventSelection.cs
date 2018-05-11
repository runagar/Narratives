using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSelection : MonoBehaviour {

    WorkloadHandler workloadHandler;
    VillageStats villageStats;
    FestivalEvent festival;

    // Event initialization
    private static int numEvents = 11;
    private MonthlyEvent[] events = new MonthlyEvent[numEvents];

    // Event selection
    private int eventPickNumber = 0;
    private int oldEvent = 0;

    private bool readyForNewEvent = true;
    private int currentMonth = 3;

    private int monthsSinceFestival = 0;

    int pickBreakAmount = 0;

    private bool firstEvent = true;

    // Event Creation in Start()
    void Start ()
    {
        monthsSinceFestival = 0;
        festival = GameObject.Find("events").GetComponent<FestivalEvent>();
        workloadHandler = GameObject.Find("VillageStatHandler").GetComponent<WorkloadHandler>();
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
        events[0] = new MonthlyEvent("Flood", 4);
        events[1] = new MonthlyEvent("Graveyard", 10000);
        events[2] = new MonthlyEvent("Nomads", 9);
        events[3] = new MonthlyEvent("Raiders", 3);
        events[4] = new MonthlyEvent("Mine", 6);
        events[5] = new MonthlyEvent("Witch", 7);
        events[6] = new MonthlyEvent("Festival", 12);
        events[7] = new MonthlyEvent("Disease", 6);
        events[8] = new MonthlyEvent("War", 24);
        events[9] = new MonthlyEvent("Blight", 7);
        events[10] = new MonthlyEvent("Forest Fire", 14);
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

    public int GetCurrentMonth()
    {
        return currentMonth;
    }
 
    // Pick an event and execute it.
    void SelectEvent()
    {
        Debug.Log("---------------------------NEW CYCLE---------------------------");
        int eventPickBreaker = 0;
        // Pick an event we did not have last cycle.
        while(eventPickNumber == oldEvent || !events[eventPickNumber].GetAvailable())
        {
            eventPickNumber = Random.Range(0, numEvents);
            eventPickBreaker++;
            if (eventPickBreaker > 30)
            {
                pickBreakAmount++;
                Debug.Log("Break - PickTimer: " + pickBreakAmount);
                break;
            }
        }

        oldEvent = eventPickNumber; // Store the event we had this time

        foreach(MonthlyEvent e in events)
        {
            e.UpdateAvailabilityTimer();
        }
        events[eventPickNumber].SetUnavilable();

        string eventName = events[eventPickNumber].GetName();

        if (villageStats.GetImprovement("Festival")) monthsSinceFestival++;
        Debug.Log("months since festival: " + monthsSinceFestival);
        if (monthsSinceFestival >= 9)
        {
            eventName = "Festival";
            monthsSinceFestival = 0;
        }
        if (firstEvent)
        {
            eventName = "First";
        }

        // Get the name of the event, and execute it in the handler.
        this.gameObject.GetComponent<EventHandler>().HandleEvent(eventName);

        workloadHandler.UpdateWorkload(currentMonth);
        villageStats.UpdateVillage(currentMonth, firstEvent);
        currentMonth++;
        if (currentMonth >= 13) currentMonth = 1;
        if (firstEvent)
        {
            firstEvent = false;
        }
    }

    void BirthEvent()
    {

        if(festival.GetBanquetState()) monthsSinceFestival++;
        
        if(monthsSinceFestival == 9)
        {

        }
    }
}
