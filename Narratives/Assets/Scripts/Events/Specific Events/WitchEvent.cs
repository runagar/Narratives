using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchEvent : MonoBehaviour {

    private int foodEff, workEff, moraleEff, popEff;
    VillageStats villageStats;

    private void Start()
    {
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
    }

    public void LaunchEvent()
    {
        // Do a flood thing. e.g. options 1 & 2, based on buildings.
        if (villageStats.GetPerson("Witch"))
        {
            // food =- 100;
            // Set the name, description and options for this event, if improvement has been build. e.g.
            // name = Flood!
            // desc = "A flood has stiken the land, but your dikes took the blunt of the blow. A small amount of food has been lost. However, the dikes are damaged, and will have to be repaired.
            // Option 1 = Repair the dikes: -20 work,
            // Option 2 = Let them fall into disrepair: Dikes are destroyed.
        }
        else
        {
            // Food =- 300;
            // Set the name, description and options for this event.
            // Flood!
            // desc = A flood has destroyed many of your crops and buildings.
            // Option 1 = Build a dike: The next flood will not hit nearly as hard, -30 work
            // Option 2 = Do nothing, the workers are needed elsewhere.
        }
    }
}
