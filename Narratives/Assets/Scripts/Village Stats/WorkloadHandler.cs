using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkloadHandler : MonoBehaviour {

    VillageStats villageStats;

    private int workload;
    private int workloadThreshold;

    public bool buildingDikes, repairingDikes;
    public bool buildingGraveyard, buildingMassGrave;

    private int dikeTimer = 0, dikeTimerEnd = 3;
    private int graveyardTimer = 0, graveyardTimerEnd = 2;
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
