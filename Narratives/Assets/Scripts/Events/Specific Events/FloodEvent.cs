using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodEvent : MonoBehaviour {

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
        if (villageStats.GetImprovement("Dike"))
        {
            buildingPresent = true;
            villageStats.SetResource("food", -75);

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Flood!";
            eventDescription = "A flood has stiken the land, but your dikes took the blunt of the blow. 75 food has been lost. However, the dikes are damaged, and will have to be repaired.";
            optionOne = "Repair the dikes.";
            optionTwo = "Let them fall.";
            optionOneTooltip = "+20 Workload for 3 months" + "\n" + "Should another flood happen the dikes will take the blunt of the blow.";
            optionTwoTooltip = "Your dikes will fall." + "\n" + "Should another flood happen, it will hit at full strength!";
        }
        else
        {
            buildingPresent = false;
            villageStats.SetResource("food", -200);

            // Set the name, description and options for this event.
            eventName = "Flood!";
            eventDescription = "A flood has stiken the land, destroying many of your crops and flooding your barn. 200 food has been lost.";
            optionOne = "Build dikes.";
            optionTwo = "Do nothing.";
            optionOneTooltip = "+30 Workload for 3 months" + "\n" + "Gain: Dikes" + "\n" + "Should another flood happen, it won't hit nearly as hard.";
            optionTwoTooltip = "Reserve your workforce." + "\n" + "Should another flood happen, it will hit just as hard!";
        }
        int currentMonth = eventSelection.GetCurrentMonth();

        if(currentMonth > 4 && currentMonth < 10)
        {
            eventDescription += "\n \n" + "The farming season has begun, and people are busy in the fields.";
            if(currentMonth > 6)
            {
                eventDescription += "\n" + "Harvest is coming in.";
            }
        }

        drawThisEvent = true;
    }

    void OptionOneA()
    {
        // Repair the dikes
        workloadHandler.repairingDikes = true;
    }

    void OptionTwoA()
    {
        // Let the dikes fall
        villageStats.RemoveImprovement("Dike");
    }

    void OptionOneB()
    {
        // Build dikes
        workloadHandler.buildingDikes = true;
        villageStats.BuildImprovement("Dike");
    }

    void OptionTwoB()
    {
        // Do nothing
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

                if(e.button == 0 && e.type == EventType.MouseUp)
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
