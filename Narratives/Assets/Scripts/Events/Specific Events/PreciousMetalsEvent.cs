using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciousMetalsEvent : MonoBehaviour {

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
        // Do a flood thing. e.g. options 1 & 2, based on buildings.
        if (villageStats.GetImprovement("Mine"))
        {
            buildingPresent = true;

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Mine Collapse!";
            eventDescription = "While extracting precious metals the mine shaft collapsed and now the workers are trapped inside.";
            optionOne = "Begin a rescuse mission.";
            optionTwo = "It is too dangerous to dig them out, let us pray for their souls.";
            optionOneTooltip = "+" + (int)(villageStats.GetResource("pop_Adults") * 0.70) + " Workload for 3 months" + "\n" + "Morale increases";
            optionTwoTooltip = "Lose the people working in the mine." + "\n" + "Morale decreases";
        }
        else
        {
            buildingPresent = false;

            // Set the name, description and options for this event.
            eventName = "Precious Metals";
            eventDescription = "Some of the women have found traces of precious metals while down at the river washing cloths.";
            optionOne = "Build a mine.";
            optionTwo = "Do nothing.";
            optionOneTooltip = "+" + (int)((villageStats.GetResource("pop_Adults")*2) * 0.25) + " Workload while working the mine" + "\n" + "Gain: Mine" + "\n" + "+1 Morale per month while working the mine.";
            optionTwoTooltip = "Reserve your workforce." + "\n" + "Morale decreases slightly";
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
        // rescue people form the mine
        villageStats.RemoveImprovement("Mine");
        workloadHandler.mineRescue = true;
        villageStats.SetResource("pop_Adults", (int)(villageStats.GetResource("pop_Adults") * 0.05));

    }

    void OptionTwoA()
    {
        // let us pray for the dead
        villageStats.RemoveImprovement("Mine");
        villageStats.SetResource("pop_Adults", (int)(villageStats.GetResource("pop_Adults") * 0.25));
        villageStats.SetResource("morale", -villageStats.GetResource("morale") / 2);
        workloadHandler.buildingMine = false;
    }

    void OptionOneB()
    {
        // Build mines
        workloadHandler.buildingMine = true;
        villageStats.BuildImprovement("Mine");
        Debug.Log("Building mine from here");
    }

    void OptionTwoB()
    {
        // Do nothing
        villageStats.SetResource("morale", -10);
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
