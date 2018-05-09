using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestFireEvent : MonoBehaviour
{
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
        eventName = "Forest Fire!";
        eventDescription = "The forest next to the village has caught on fire! Nevermind how it started, protect the village!";
        optionOne = "Protect the houses!";
        optionTwo = "Protect the granary!";
        optionOneTooltip = "You granary will burn down, and most of your food will be lost. \n -5 Morale.";
        optionTwoTooltip = "Several homes will burn down, with a risk of people being caught inside. \n -5 Morale.";

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
        villageStats.SetResource("Morale", -5);
        int foodLost = (int)(villageStats.GetResource("food") * 0.6);
        villageStats.SetResource("food", -foodLost);
    }

    void OptionTwo()
    {
        villageStats.SetResource("Morale", -5);
        villageStats.SetResource("pop_Adults", -(Random.Range(5, 30)));
        villageStats.SetResource("pop_Children", -(Random.Range(3, 18)));
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