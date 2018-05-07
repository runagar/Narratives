﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {

    EventSelection eventSelection;
    VillageStats villageStats;
    GameObject eventStore;

	// Use this for initialization
	void Start () {
        eventSelection = GameObject.Find("EventHandler").GetComponent<EventSelection>();
        villageStats = GameObject.Find("VillageStatHandler").GetComponent<VillageStats>();
        eventStore = GameObject.Find("events");
	}

    public void HandleEvent(string eventName)
    {
        // TODO: Pick and resolve the correct event.
        switch (eventName)
        {
            case "Flood":
                eventStore.GetComponent<FloodEvent>().LaunchEvent();
                break;
            case "Witch":
                eventStore.GetComponent<WitchEvent>().LaunchEvent();
                break;
            case "Graveyard":
                eventStore.GetComponent<GraveyardEvent>().LaunchEvent();
                break;
            case "Nomads":
                eventStore.GetComponent<NomadsEvent>().LaunchEvent();
                break;
            case "Raiders":
                if(villageStats.GetResource("raiders")>0) eventStore.GetComponent<RaidersEvent>().LaunchEvent();
                else eventStore.GetComponent<NomadsEvent>().LaunchEvent();
                break;
            default:
                break;
        }

        eventSelection.SetReadyForNewEvent();
    }
}
