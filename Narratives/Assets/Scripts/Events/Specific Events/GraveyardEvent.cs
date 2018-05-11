using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardEvent : MonoBehaviour {

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

    private Rect eventOptionAtip, eventOptionBtip;

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
        eventOptionBWindow = new Rect(eventWindowStartPosX + eventOptionWindowWidth + 5, eventWindowStartPosY + eventNameWindowHeight + eventDescriptionWindowHeight, eventOptionWindowWidth, eventOptionWindowHeight);
        eventOptionAtip = new Rect(eventWindowStartPosX, eventWindowStartPosY + eventNameWindowHeight + eventDescriptionWindowHeight + eventOptionWindowHeight, eventOptionWindowWidth, Screen.height / 5);
        eventOptionBtip = new Rect(eventWindowStartPosX + eventOptionWindowWidth + 5, eventWindowStartPosY + eventNameWindowHeight + eventDescriptionWindowHeight + eventOptionWindowHeight, eventOptionWindowWidth, Screen.height / 5);
    }

    public void LaunchEvent()
    {
        // Set the name, description and options for this event, if improvement has been build. e.g.
        eventName = "Corpses.";
        eventDescription = "The village priest approaches you. He tells you that the old graveyard does not have enough room for the recent dead, and implores you to build a new graveyard.";
        optionOne = "Build a graveyard.";
        optionTwo = "A mass grave will do..";
        optionOneTooltip = "+20 Workload for 2 months" + "\n" + "Morale increases.";
        optionTwoTooltip = "+5 Workload for 1 month." + "\n" + "Morale significantly decreases.";
        
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

    void OptionOne()
    {
        villageStats.BuildImprovement("Graveyard");
        villageStats.SetResource("Morale", 10);
        workloadHandler.buildingGraveyard = true;
    }

    void OptionTwo()
    {
        villageStats.SetResource("Morale", -30);
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
            GUI.Box(new Rect(eventOptionAtip), optionOneTooltip, skin.GetStyle("eventWindowDescription"));
            GUI.Box(new Rect(eventOptionBtip), optionTwoTooltip, skin.GetStyle("eventWindowDescription"));

            if (eventOptionAWindow.Contains(e.mousePosition))
            {
                tooltip = optionOneTooltip;

                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    OptionOne();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }
            else if (eventOptionBWindow.Contains(e.mousePosition))
            {
                tooltip = optionTwoTooltip;

                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    OptionTwo();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }
        }
    }
}