using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodEvent : MonoBehaviour {

    private int foodEff, workEff, moraleEff, popEff;
    VillageStats villageStats;
    bool drawThisEvent = true;

    public GUISkin skin;

    private int eventWindowStartPosX, eventWindowStartPosY, eventWindowHeight, eventWindowWidth;
    private Rect eventWindow;


    private string eventName, eventDescription, optionOne, optionTwo;

    private void Start()
    {
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
        eventWindowStartPosX = Screen.width / 4;
        eventWindowStartPosY = Screen.height / 4;
        eventWindowWidth = Screen.width / 2;
        eventWindowHeight = Screen.height / 2;
        eventWindow = new Rect(eventWindowStartPosX, eventWindowStartPosY, eventWindowWidth, eventWindowHeight);
    }

    public void LaunchEvent()
    {
        // Do a flood thing. e.g. options 1 & 2, based on buildings.
        if (villageStats.GetImprovement("Dike"))
        {
            villageStats.SetResource("food", -50);

            // Set the name, description and options for this event, if improvement has been build. e.g.
            eventName = "Flood!";
            eventDescription = "A flood has stiken the land, but your dikes took the blunt of the blow. A small amount of food has been lost. However, the dikes are damaged, and will have to be repaired.";
            optionOne = "Repair the dikes.";
            optionTwo = "Let them fall.";
        }
        else
        {
            villageStats.SetResource("food", -150);

            // Set the name, description and options for this event.
            eventName = "Flood!";
            eventDescription = "A flood has stiken the land, destroying many of your crops and flooding your barn. A large amount of food has been lost.";
            optionOne = "Build dikes.";
            optionTwo = "Do nothing.";
        }

        drawThisEvent = true;
    }

    void OptionOneA()
    {
        // Repair the dikes
        villageStats.SetResource("work", -20);
    }

    void OptionTwoA()
    {
        // Let the dikes fall
        villageStats.RemoveImprovement("Dike");
    }

    void OptionOneB()
    {
        // Build dikes
        villageStats.SetResource("work", -30);
        villageStats.BuildImprovement("Dike");
    }

    void OptionTwoB()
    {
        // Do nothing
    }

    void OnGUI()
    {
        GUI.skin = skin;
        skin.GetStyle("EventWindow").wordWrap = true;

        if (drawThisEvent)
        {
            GUI.Box(new Rect(eventWindow), eventName);
        }
    }
}
