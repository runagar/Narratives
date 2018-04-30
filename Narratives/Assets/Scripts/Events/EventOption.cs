using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOption : MonoBehaviour {

    private string description;
    private int populationEffect, moraleEffect, foodEffect;

    EventOption (string desc, int popEff, int moraleEff, int foodEff)
    {
        this.description = desc;
        this.populationEffect = popEff;
        this.moraleEffect = moraleEff;
        this.foodEffect = foodEff;
    }
}
