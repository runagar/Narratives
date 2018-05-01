using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event {

    private string name, description, optionOneDesc, optionTwoDesc;

    public Event(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }
}
