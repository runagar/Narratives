using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkloadHandler : MonoBehaviour {

    VillageStats villageStats;

    private int workload;
    private int workloadThreshold;

    public bool buildingDikes, repairingDikes, reparingBarricade;
    public bool buildingGraveyard, buildingMassGrave, buildingBarricade;

    private int dikeTimer = 0, dikeTimerEnd = 3;
    private int graveyardTimer = 0, graveyardTimerEnd = 2;
    private int barricadeTimer = 0, barricadeTimerEnd = 3;
    private void Start()
    {
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
    }

    public void UpdateWorkload(int currentMonth)
    {
        workload = 0;
        workloadThreshold = villageStats.GetResource("pop_Adults")*2;

        if(currentMonth > 4 && currentMonth < 10)
        {
            int farmingWorkload = (int)workloadThreshold / 2;
            workload += farmingWorkload;
        }

        if (buildingBarricade)
        {
            workload += 30;
            barricadeTimer++;
            if(barricadeTimer >= barricadeTimerEnd)
            {
                barricadeTimerEnd = 0;
                buildingBarricade = false;
            }
        }

        if (reparingBarricade)
        {
            workload += 20;
            dikeTimer++;
            if (barricadeTimer >= barricadeTimerEnd-1)
            {
                barricadeTimer = 0;
                reparingBarricade = false;
            }
        }

        if (buildingGraveyard)
        {
            workload += 20;
            graveyardTimer++;
            if(graveyardTimer >= graveyardTimerEnd)
            {
                graveyardTimer = 0;
                buildingGraveyard = false;
            }
        }

        if (buildingMassGrave) workload += 5;

        if (buildingDikes)
        {
            workload += 30;
            dikeTimer++;
            if (dikeTimer >= dikeTimerEnd)
            {
                dikeTimer = 0;
                buildingDikes = false;
            }
        }

        if (repairingDikes)
        {
            workload += 20;
            dikeTimer++;
            if (dikeTimer >= dikeTimerEnd)
            {
                dikeTimer = 0;
                repairingDikes = false;
            }
        }
        villageStats.SetResource("work", workload);
        if (workload > workloadThreshold) villageStats.SetResource("morale", -5);
    }
}
