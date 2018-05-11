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

            randAdults = Random.Range(2, 10);

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Raiders!";
            eventDescription = "The village is approched wandering band of " + raiders + " raiders. Your barricades protect your people while they fight off the raiders, but are heavily damaged in the battle. \n " + randAdults + " people were killed in the battle.";
            optionOne = "Repair the barricades.";
            optionTwo = "Let the barricades fall.";
            optionOneTooltip = "Your barricades are repaired \n" + "+20 Workload for 2 months.";
            optionTwoTooltip = "Your barricades are destroyed. \n" + "-5 Morale.";
        }
        else
        {
            buildingPresent = false;
            randAdults = Random.Range(2, 20);

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Raiders!";
            eventDescription = "The village is approched wandering band of " + raiders + " raiders. Your people defend themselves as well as they can, but the raiders make off with " + raiders*2 + " food, and kill " + randAdults + " of your people in the process.";
            optionOne = "We need better defences!";
            optionTwo = "Back to work.";
            optionOneTooltip = "Build barricades to hold off the raiders next time. \n" + " +30 Workload for 3 months.";
            optionTwoTooltip = "Reserve your workforce. \n" + " -5 Morale.";
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
        villageStats.SetResource("pop_Adults", -randAdults);
        workloadHandler.reparingBarricade = true;
    }

    void OptionTwoA()
    {
        // Let the barricade fall
        villageStats.SetResource("raiders", -Random.Range(5, raiders));
        villageStats.SetResource("pop_Adults", -randAdults);
        villageStats.RemoveImprovement("Barricade");
        villageStats.SetResource("morale", -5);
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
        villageStats.SetResource("morale", -5);
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
