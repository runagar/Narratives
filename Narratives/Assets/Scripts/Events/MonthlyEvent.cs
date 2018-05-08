using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthlyEvent {

    private string name;
    private bool available = true;
    private int availableTimer = 0;
    private int availableTimerThreshold;

    public MonthlyEvent(string name, int availabilityThreshold)
    {
        this.name = name;
        this.availableTimerThreshold = availabilityThreshold;
    }

    public string GetName()
    {
        return name;
    }

    public bool GetAvailable()
    {
        return available;
    }

    public void UpdateAvailabilityTimer()
    {
        if(availableTimer > 0) availableTimer--;
        if(available == false && availableTimer == 0) available = true;
    }

    public void SetUnavilable()
    {
        availableTimer = availableTimerThreshold;
        available = false;
    }
}
