﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseEvent : MonoBehaviour {

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
        // Set the name, description and options for this event, if improvement has been build. e.g.
        eventName = "Disease.";
        eventDescription = "A disease spreading through the land have struck our village.";
        optionOne = "Quarantine the sick.";
        optionTwo = "Pray for aid.";
        optionOneTooltip = "+15 Workload for 3 months" + "\n" + "Morale decreases and people die.";
        optionTwoTooltip = "+10 Workload for 1 month." + "\n" + "People die";

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
        //quarantine the sick villagers
        villageStats.SetResource("morale", -30);
        workloadHandler.quaratineTheSick = true;
        if (villageStats.GetImprovement("Witch"))
        {
            villageStats.SetResource("pop_Adults", -villageStats.GetResource("pop_Adults") / 6);
            villageStats.SetResource("pop_Children", -villageStats.GetResource("pop_Children") / 6);
        }
        else
        {
            villageStats.SetResource("pop_Adults", -villageStats.GetResource("pop_Adults") / 6);
            villageStats.SetResource("pop_Children", -villageStats.GetResource("pop_Children") / 6);
        }
    }

    void OptionTwo()
    {
        //pray the sick villagers
        villageStats.SetResource("morale", -15);
        workloadHandler.prayForTheSick = true;
        if (villageStats.GetImprovement("Witch"))
        {
            villageStats.SetResource("pop_Adults", -villageStats.GetResource("pop_Adults") / 4);
            villageStats.SetResource("pop_Children", -villageStats.GetResource("pop_Children") / 4);
        }
        else
        {
            villageStats.SetResource("pop_Adults", -villageStats.GetResource("pop_Adults") / 3);
            villageStats.SetResource("pop_Children", -villageStats.GetResource("pop_Children") / 3);
        }
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
                    OptionOne();
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
                    OptionTwo();
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
