using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event {

    private string name, description, optionOneDesc, optionTwoDesc;

    public Event(string name, string description, string opOne, string opTwo)
    {
        this.name = name;
        this.description = description;
        this.optionOneDesc = opOne;
        this.optionTwoDesc = opTwo;
    }
}
