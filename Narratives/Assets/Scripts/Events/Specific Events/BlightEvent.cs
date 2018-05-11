using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlightEvent : MonoBehaviour {

    VillageStats villageStats;
    EventSelection eventSelection;
    WorkloadHandler workloadHandler;

    bool drawThisEvent = false;

    public GUISkin skin;

    private int eventWindowStartPosX, eventWindowStartPosY, eventNameWindowHeight, eventWindowWidth;
    private int eventDescriptionWindowHeight, eventOptionWindowHeight, eventOptionWindowWidth;
    private Rect eventNameWindow, eventDescriptionWindow, eventOptionAWindow, eventOptionBWindow;

    private string eventName, eventDescription, optionOne, optionTwo, optionOneTooltip, optionTwoTooltip, tooltip;
    private bool showTooltip = false, cropsBlight = false;

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

    public bool GetBanquetState()
    {
        return cropsBlight;
    }

    public void LaunchEvent()
    {
        // Do a flood thing. e.g. options 1 & 2, based on buildings.
        if (eventSelection.GetCurrentMonth() < 7 || eventSelection.GetCurrentMonth() > 9)
        {
            villageStats.SetResource("food", -(villageStats.GetResource("food") / 2));
            cropsBlight = false;

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Rats!";
            eventDescription = "Rats have gotten into our food supplies and we have lost half our food";
            optionOne = "Ration our food.";
            optionTwo = "Continue as usual.";
            optionOneTooltip = "Morale decreases";
            optionTwoTooltip = "Ignore the situation and continue as normal";
        }
        else
        {
            villageStats.BuildImprovement("Blight");
            cropsBlight = true;

            // Set the name, description and options for this event.
            eventName = "Crops Blight!";
            eventDescription = "The fields have been hit by a blight destroying our crops";
            optionOne = "Ration our food.";
            optionTwo = "Continue as usual.";
            optionOneTooltip = "Morale decreases";
            optionTwoTooltip = "Ignore the situation and continue as normal";
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
        // Blight Ration our food;
        villageStats.BuildImprovement("Ration");
        
    }

    void OptionTwoA()
    {
        // Blight continue as usual
    }

    void OptionOneB()
    {
        // Rats! Ration our food
        villageStats.BuildImprovement("Ration");

    }

    void OptionTwoB()
    {
        //Rats! continue as usual
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
                showTooltip = true;
                tooltip = optionOneTooltip;

                if (e.button == 0 && e.type == EventType.MouseUp)
                {
                    if (cropsBlight) OptionOneA();
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
                    if (cropsBlight) OptionTwoA();
                    else OptionTwoB();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }
            else
            {
                //If the tooltip string is blank, stop drawing the tooltip
                tooltip = "";
            }

            
        }
    }

    void DrawTooltip()
    {
        float toolTipHeight = tooltip.Length;

        GUI.Box(new Rect(Event.current.mousePosition.x - 20, Event.current.mousePosition.y, 200, toolTipHeight), tooltip, skin.GetStyle("tooltipBackground"));

    }
}