using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkloadHandler : MonoBehaviour {

    VillageStats villageStats;

    private int workload;
    private int workloadThreshold;

    public bool buildingDikes, repairingDikes;

    private int dikeTimer = 0, dikeTimerEnd = 3;

    private void Start()
    {
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
    }

    public void UpdateWorkload()
    {
        workload = 0;
        workloadThreshold = villageStats.GetResource("pop_Adults")*2;

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
