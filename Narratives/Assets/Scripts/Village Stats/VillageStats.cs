﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStats : MonoBehaviour {

    private static int improvementCount = 20;
    private string[] improvements = new string[improvementCount];

    private static int peopleCount = 5;
    private string[] people = new string[peopleCount];

    private int food = 500,
                foodConsumption = 0,
                work = 0,
                workThreshold = 0,
                morale = 100,
                population_Children = 20,
                population_Adults = 90,
                raiders = 5,
                soldiers = 0;

    public GUISkin skin;

    private Rect statBoxRect;
    private int statBoxStartPosX,
                statBoxStartPosY,
                statBoxWidth,
                statBoxHeight;

    private string statBoxText;

    LossScript lossScript;

    public bool gameIsLost = false;

    // Use this for initialization
    void Start()
    {
        statBoxStartPosX = Screen.width / 40;
        statBoxStartPosY = Screen.height / 20;
        statBoxWidth = Screen.width / 6;
        statBoxHeight = Screen.height / 8;
        statBoxRect = new Rect(statBoxStartPosX, statBoxStartPosY, statBoxWidth, statBoxHeight);

        for (int i = 0; i < improvementCount; i++) improvements[i] = "";
        for (int i = 0; i < peopleCount; i++) people[i] = "";
        improvements[0] = "Barn";

        lossScript = this.gameObject.GetComponent<LossScript>();
    }

    // Update is called once per frame
    void Update()
    {
        foodConsumption = (int)((population_Adults + population_Children) / 2.0);
        workThreshold = population_Adults * 2;
        statBoxText = "Food: " + food + " (" + foodConsumption + ") " + "\n" + "Workload: " + work + "/" + workThreshold + "\n" + "Morale: " + morale + "\n" + "Population: " + population_Children + " / " + population_Adults;
    }

    //Look for a specific improvement in the list. Return true if it is present.
    public bool GetImprovement(string improvement)
    {
        for (int i = 0; i < improvementCount; i++)
        {
            if (improvements[i] == improvement)
            {
                return true;
            }
        }
        return false;
    }

    public void BuildImprovement(string improvement)
    {
        for (int i = 0; i < improvementCount; i++)
        {
            if (improvements[i] == "")
            {
                improvements[i] = improvement;
                break;
            }
        }
    }

    public void RemoveImprovement(string improvement)
    {
        for (int i = 0; i < improvementCount; i++)
        {
            if (improvements[i] == improvement)
            {
                improvements[i] = "";
                break;
            }
        }
    }

    //Look for a specific improvement in the list. Return true if it is present.
    public bool GetPerson(string person)
    {
        for (int i = 0; i < peopleCount; i++)
        {
            if (people[i] == person)
            {
                return true;
            }
        }
        return false;
    }

    public void SetPerson(string person)
    {
        for (int i = 0; i < peopleCount; i++)
        {
            if (people[i] == "")
            {
                people[i] = person;
                break;
            }
        }
    }

    public void RemovePerson(string person)
    {
        for (int i = 0; i < peopleCount; i++)
        {
            if (people[i] == person)
            {
                people[i] = "";
                break;
            }
        }
    }

    public void SetResource(string resource, int i)
    {
        switch (resource)
        {
            case "food":
                food += i;
                if (food < 0) food = 0;
                break;
            case "work":
                work = i;
                break;
            case "morale":
                morale += i;
                if (morale <= 0)
                {
                    gameIsLost = true;
                    lossScript.LoseGame("Morale", false);
                }
                break;
            case "pop_Adults":
                population_Adults += i;
                bool children = false;
                if (GetResource("pop_Children") > 0) children = true;
                if (population_Adults < 0)
                {
                    gameIsLost = true;
                    lossScript.LoseGame("Population", children);
                }
                break;
            case "pop_Children":
                population_Children += i;
                if (population_Children == 0) SetResource("morale", -15);
                break;
            case "raiders":
                raiders += i;
                break;
            case "soldiers":
                soldiers += i;
                break;
            default:
                break;
        }
    }

    public int GetResource(string resource)
    {
        switch (resource)
        {
            case "food":
                return food;
            case "work":
                return work;
            case "morale":
                return morale;
            case "pop_Adults":
                return population_Adults;
            case "pop_Children":
                return population_Children;
            case "raiders":
                return raiders;
            case "soldiers":
                return soldiers;
            default:
                break;
        }
        return 0;
    }

    public void UpdateVillage(int currentMonth)
    {

        if (currentMonth >= 11 && GetImprovement("Blight"))
        {
            RemoveImprovement("Blight");
            RemoveImprovement("Ration");
        }

        if (raiders <= 5) raiders = 5;
        // If the harvesting months are here
        if((currentMonth > 6 && currentMonth < 10) && !GetImprovement("Blight"))
        {
            food += foodConsumption * 4;
        }
        else
        {
            food += (int)(foodConsumption * 0.75);
        }

        // Consume food
        if (GetImprovement("Ration"))
        {
            food -= foodConsumption / 3;
            SetResource("morale", -1);
        }
        else
        {
            food -= foodConsumption;
        }
        if (food <= 0)
        {
            food = 0;
            SetResource("pop_Adults", -3);
            SetResource("pop_Children", -1);
            SetResource("morale", -10);
        }

        if (GetImprovement("Mine"))
        {
            SetResource("morale", 1);
        }

        if (work > workThreshold) SetResource("morale", -5);
        if (morale < 0) morale = 0;
        if (morale > 100) morale = 100;
        if (population_Children < 0) population_Children = 0;
        if (population_Adults < 0) population_Adults = 0;
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        skin.GetStyle("StatBox").wordWrap = true;

        GUI.Box(new Rect(statBoxRect), statBoxText, skin.GetStyle("StatBox"));
    }
}
