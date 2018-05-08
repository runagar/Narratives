using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidersEvent : MonoBehaviour {

    VillageStats villageStats;
    EventSelection eventSelection;
    WorkloadHandler workloadHandler;

    bool drawThisEvent = false;

    public GUISkin skin;

    private int eventWindowStartPosX, eventWindowStartPosY, eventNameWindowHeight, eventWindowWidth;
    private int eventDescriptionWindowHeight, eventOptionWindowHeight, eventOptionWindowWidth;
    private Rect eventNameWindow, eventDescriptionWindow, eventOptionAWindow, eventOptionBWindow;

    private string eventName, eventDescription, optionOne, optionTwo, optionOneTooltip, optionTwoTooltip, tooltip;
    private bool showTooltip = false, buildingPresent;

    //Random adults lost and raiders
    private int randAdults;
    private int raiders;

    private void Start()
    {
        eventSelection = this.gameObject.GetComponentInParent<EventSelection>();
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
        workloadHandler = GameObject.Find("VillageStatHandler").GetComponent<WorkloadHandler>();

        eventWindowStartPosX = Screen.width / 4;
        eventWindowStartPosY = Screen.height / 4;
        eventWindowWidth = Screen.width / 2;
        eventNameWindowHeight = Screen.height / 12;
        eventDescriptionWindowHeight = Screen.height / 3;
        eventOptionWindowHeight = Screen.height / 12;
        eventOptionWindowWidth = Screen.width / 4;
        eventNameWindow = new Rect(eventWindowStartPosX, eventWindowStartPosY, eventWindowWidth, eventNameWindowHeight);
        eventDescriptionWindow = new Rect(eventWindowStartPosX, eventWindowStartPosY + eventNameWindowHeight, eventWindowWidth, eventDescriptionWindowHeight);
        eventOptionAWindow = new Rect(eventWindowStartPosX, eventWindowStartPosY + eventNameWindowHeight + eventDescriptionWindowHeight, eventOptionWindowWidth, eventOptionWindowHeight);
        eventOptionBWindow = new Rect(eventWindowStartPosX + eventOptionWindowWidth, eventWindowStartPosY + eventNameWindowHeight + eventDescriptionWindowHeight, eventOptionWindowWidth, eventOptionWindowHeight);
    }

    public void LaunchEvent()
    {
        raiders = villageStats.GetResource("raiders");
        if (villageStats.GetImprovement("Barricade"))
        {
            buildingPresent = true;
            //set the name and option for this case of the event.
            randAdults = Random.Range(0, 20);
            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Raiders are here.";
            eventDescription = "The village is approched wandering band of " + raiders + " raiders, but it won't end like last time.";
            optionOne = "Defend the village with your life and let the barricade fall.";
            optionTwo = "Defend the village and repair the barricade after they leave";
            optionOneTooltip = "- " + randAdults + " adults" + "\n" + "Morale and Food decreases.";
            optionTwoTooltip = "Lose some adults. +20 Workload for 3 months";
        }
        else
        {
            buildingPresent = false;
            randAdults = Random.Range(0, 20);
            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Raiders are here.";
            eventDescription = "The village is approched wandering band of " + raiders + " raiders.";
            optionOne = "Wait them out and build a barricade after they leave";
            optionTwo = "Defend the village with your life.";
            optionOneTooltip = "- " + randAdults + " adults" + "\n" + "Morale and Food decreases.";
            optionTwoTooltip = "Morale and Food significantly decreases. +30 Workload for 3 months";
        }
       
        int currentMonth = eventSelection.GetCurrentMonth();

        if (currentMonth > 4 && currentMonth < 10)
        {
            eventDescription += "\n \n" + "The farming season has begun, and people are busy in the fields.";
            if (currentMonth > 6)
            {
                eventDescription += "\n" + "Harvest is coming in.";
            }
        }

        drawThisEvent = true;
    }

    void OptionOneA()
    {
        // Repair the barricade
        villageStats.SetResource("raiders", -Random.Range(5, raiders));
        villageStats.SetResource("pop_Adults", (int)(-randAdults * 0.75));
        workloadHandler.reparingBarricade = true;
    }

    void OptionTwoA()
    {
        // Let the barricade fall
        villageStats.SetResource("raiders", -Random.Range(5, raiders));
        villageStats.SetResource("pop_Adults", (int)(-randAdults * 0.75));
        villageStats.RemoveImprovement("Barricade");
    }

    void OptionOneB()
    {
        // Build barricade
        workloadHandler.buildingBarricade = true;
        villageStats.BuildImprovement("Barricade");
        villageStats.SetResource("food", -(raiders * 2));
        villageStats.BuildImprovement("Barricade");
    }

    void OptionTwoB()
    {
        // Defend the village wit your lives witout barricades
        villageStats.SetResource("raiders", -Random.Range(0, raiders));
        villageStats.SetResource("pop_Adults", -randAdults);
        villageStats.SetResource("food", -(raiders * 2));

    }

    void OnGUI()
    {
        GUI.skin = skin;
        skin.GetStyle("eventWindowDescription").wordWrap = true;

        Event e = Event.current;

        if (drawThisEvent)
        {
            GUI.Box(new Rect(eventNameWindow), eventName, skin.GetStyle("eventWindowName"));
            GUI.Box(new Rect(eventDescriptionWindow), eventDescription, skin.GetStyle("eventWindowDescription"));
            GUI.Box(new Rect(eventOptionAWindow), optionOne, skin.GetStyle("eventWindowOption"));
            GUI.Box(new Rect(eventOptionBWindow), optionTwo, skin.GetStyle("eventWindowOption"));

            if (eventOptionAWindow.Contains(e.mousePosition))
            {
                showTooltip = true;
                tooltip = optionOneTooltip;

                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    if (buildingPresent) OptionOneA();
                    else OptionOneB();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }
            else if (eventOptionBWindow.Contains(e.mousePosition))
            {
                showTooltip = true;
                tooltip = optionTwoTooltip;

                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    if (buildingPresent) OptionTwoA();
                    else OptionTwoB();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }

            if (showTooltip)
            {
                DrawTooltip();
            }
        }
    }

    void DrawTooltip()
    {


    }
}
