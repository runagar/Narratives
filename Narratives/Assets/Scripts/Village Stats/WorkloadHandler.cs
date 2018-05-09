using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkloadHandler : MonoBehaviour {

    VillageStats villageStats;

    private int workload;
    private int workloadThreshold;

    public bool repairingDikes, reparingBarricade, mineRescue;
    public bool buildingDikes, buildingGraveyard, buildingMassGrave, buildingBarricade, buildingMine;
    public bool quaratineTheSick, prayForTheSick;

    private int dikeTimer = 0, dikeTimerEnd = 3;
    private int graveyardTimer = 0, graveyardTimerEnd = 2;
    private int barricadeTimer = 0, barricadeTimerEnd = 3;
    private int mineTimer = 0, mineTimerEnd = 1;
    private int quarantineTimer = 0, quarantineTimerEnd = 3; 
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

        if (quaratineTheSick)
        {
            workload += 15;
            quarantineTimer++;
            {
                if(quarantineTimer >= quarantineTimerEnd)
                {
                    quarantineTimer = 0;
                    quaratineTheSick = false;
                }
            }
        }

        if (prayForTheSick)
        {
            workload += 10;
            quarantineTimer++;
            {
                if (quarantineTimer >= quarantineTimerEnd-2)
                {
                    quarantineTimer = 0;
                    prayForTheSick = false;
                }
            }
        }

        if (buildingMine)
        {
            workload += (int)((villageStats.GetResource("pop_Adults") * 2) * 0.25);
        }

        if (mineRescue)
        {
            workload += (int)(villageStats.GetResource("pop_Adults") * 0.70);
            mineTimer++;
            if(mineTimer >= mineTimerEnd)
            {
                mineTimer = 0;
                mineRescue = false;
                buildingMine = false;
            }
        }

        if (buildingBarricade)
        {
            workload += 30;
            barricadeTimer++;
            if(barricadeTimer >= barricadeTimerEnd)
            {
                barricadeTimer = 0;
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
