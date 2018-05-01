using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageStats : MonoBehaviour {

    private static int improvementCount = 20;
    private string[] improvements = new string[improvementCount];

    private static int peopleCount = 5;
    private string[] people = new string[peopleCount];

    private int food, work, morale, population;

    // Use this for initialization
    void Start()
    {
        improvements[0] = "Barn";
    }

    // Update is called once per frame
    void Update()
    {

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
        for (int i = 0; i < improvementCount; i++)
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
                break;
            case "work":
                work += i;
                break;
            case "morale":
                morale += i;
                break;
            case "population":
                population += i;
                break;
            default:
                break;
        }
    }
}
