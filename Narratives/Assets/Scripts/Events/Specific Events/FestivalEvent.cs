using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestivalEvent : MonoBehaviour {

    VillageStats villageStats;
    EventSelection eventSelection;
    WorkloadHandler workloadHandler;

    bool drawThisEvent = false;

    public GUISkin skin;

    private int eventWindowStartPosX, eventWindowStartPosY, eventNameWindowHeight, eventWindowWidth;
    private int eventDescriptionWindowHeight, eventOptionWindowHeight, eventOptionWindowWidth;
    private Rect eventNameWindow, eventDescriptionWindow, eventOptionAWindow, eventOptionBWindow;

    private string eventName, eventDescription, optionOne, optionTwo, optionOneTooltip, optionTwoTooltip, tooltip;
    private bool showTooltip = false, banquetHeld = false;

    private int childrenBorn;

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

    public bool GetBanquetState()
    {
        return banquetHeld;
    }

    public void LaunchEvent()
    {
        // Do a flood thing. e.g. options 1 & 2, based on buildings.
        if (villageStats.GetImprovement("Festival"))
        {
            villageStats.RemoveImprovement("Festival");

            banquetHeld = false;
            childrenBorn = Random.Range(15,(int)(villageStats.GetResource("pop_Adult") / 4));
            villageStats.SetResource("pop_Children", childrenBorn);
            villageStats.SetResource("morale", childrenBorn);

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Birhtrates are up";
            eventDescription = "Due to the banquet held some time ago the birthrates are up. " + childrenBorn + " children are born";
            optionOne = "Let the families keep the new children.";
            optionTwo = "Leave the Children in the woods.";
            optionOneTooltip = "+" + childrenBorn + " children" + "\n" + "morale increases";
            optionTwoTooltip = "We do not have enough food to support the increase in children" + "\n" +"Significant decrease in morale.";
        }
        else
        {
            banquetHeld = true;

            // Set the name, description and options for this event.
            eventName = "Banquet";
            eventDescription = "The villagers would like to hold a banquet.";
            optionOne = "We have enough to hold a banquet."; 
            optionTwo = "We do not have the supplies at the current time.";
            optionOneTooltip = "Morale increases." + "\n" + "Increase in children after some time";
            optionTwoTooltip = "Morale decreases";
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
        // Hold a Festival
        villageStats.SetResource("morale", + 10);
        villageStats.SetResource("food", -50);
        villageStats.BuildImprovement("Festival");
    }

    void OptionTwoA()
    {
        // We do not have the supplies
        villageStats.SetResource("morale", -20);
    }

    void OptionOneB()
    {
        // Keep the children
        villageStats.SetResource("morale", +10);
        villageStats.SetResource("pop_Children", childrenBorn);
    }

    void OptionTwoB()
    {
        // Leave them in the woods
        villageStats.SetResource("morale", -50);
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
                    if (banquetHeld) OptionOneA();
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
                    if (banquetHeld) OptionTwoA();
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
