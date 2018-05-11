using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarEvent : MonoBehaviour {

    VillageStats villageStats;
    EventSelection eventSelection;
    WorkloadHandler workloadHandler;

    bool drawThisEvent = false;

    public GUISkin skin;

    private int eventWindowStartPosX, eventWindowStartPosY, eventNameWindowHeight, eventWindowWidth;
    private int eventDescriptionWindowHeight, eventOptionWindowHeight, eventOptionWindowWidth;
    private Rect eventNameWindow, eventDescriptionWindow, eventOptionAWindow, eventOptionBWindow;

    private string eventName, eventDescription, optionOne, optionTwo, optionOneTooltip, optionTwoTooltip, tooltip;
    private bool showTooltip = false, war = false;

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
        if (villageStats.GetPerson("Soldier"))
        {
            villageStats.RemovePerson("Soldier");

            war = false;

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "War";
            eventDescription = "The war is over and some of the villagers who went to war have returned.";
            optionOne = "Celebrate the return of the soldiers.";
            optionTwo = "Give the returned soldiers a warm welcome.";
            optionOneTooltip = "-50 food" + "\n" + "morale increases";
            optionTwoTooltip = "-30 food" + "\n" + "morale increases.";
        }
        else
        {
            war = true;

            // Set the name, description and options for this event.
            eventName = "War";
            eventDescription = "War is coming to the frontier and the village has been asked to send troops to the kings army.";
            optionOne = "Send some young men to join the war.";
            optionTwo = "Ignore the petition for people.";
            optionOneTooltip = "Morale increases" + "\n" + "Lose some people";
            optionTwoTooltip = "Morale decrease." + "\n" + "There will be a potential increase in raiders";
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
        // Send som people to war
        villageStats.SetResource("morale", +10);
        villageStats.SetPerson("Soldier");
        villageStats.SetResource("soldiers", +(int)(villageStats.GetResource("pop_Adults") * 0.30));
        villageStats.SetResource("pop_Adults", -(int)(villageStats.GetResource("pop_Adults") * 0.30));
    }

    void OptionTwoA()
    {
        // ignore the petition
        villageStats.SetResource("morale", -20);
        villageStats.SetResource("raiders", +20);
    }

    void OptionOneB()
    {
        // Celebrate the return of the soldiers.
        villageStats.SetResource("morale", +15);
        villageStats.SetResource("food", -50);
        villageStats.SetResource("pop_Adults", Random.Range((int)(villageStats.GetResource("soldiers") * 0.25),(int)(villageStats.GetResource("soldiers") * 0.75)));
        villageStats.SetResource("soldiers", -villageStats.GetResource("soldiers"));
    }

    void OptionTwoB()
    {
        // Celebrate the return of the soldiers.
        villageStats.SetResource("morale", +10);
        villageStats.SetResource("food", -30);
        villageStats.SetResource("pop_Adults", Random.Range((int)(villageStats.GetResource("soldiers") * 0.25), (int)(villageStats.GetResource("soldiers") * 0.75)));
        villageStats.SetResource("soldiers", -villageStats.GetResource("soldiers"));
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
                    if (war) OptionOneA();
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
                    if (war) OptionTwoA();
                    else OptionTwoB();
                    drawThisEvent = false;
                    eventSelection.SetReadyForNewEvent();
                }
            }else
            {
                //If the tooltip string is blank, stop drawing the tooltip
                tooltip = "";
            }

            if (showTooltip)
            {
                DrawTooltip();
            }
        }
    }

    void DrawTooltip()
    {
        float toolTipHeight = tooltip.Length;

        GUI.Box(new Rect(Event.current.mousePosition.x - 20, Event.current.mousePosition.y, 200, toolTipHeight), tooltip, skin.GetStyle("tooltipBackground"));

    }
}
