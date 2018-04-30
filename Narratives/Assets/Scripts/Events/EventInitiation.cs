using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInitiation : MonoBehaviour {

    private Event[] events = new Event[12];

	// Use this for initialization
	void Start () {
        events[0] = new Event("name", "desc", "optionOne", "optionTwo");
        events[1] = new Event("name", "desc", "optionOne", "optionTwo");
	}
}
