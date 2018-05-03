using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementHandler : MonoBehaviour {

    private static int improvementCount = 20;
    private string[] improvements = new string[improvementCount];


	// Use this for initialization
	void Start () {
        improvements[0] = "Barn";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Look for a specific improvement in the list. Return true if it is present.
    public bool GetImprovement(string improvement)
    {
        for (int i = 0; i < improvementCount; i++)
        {
            if (improvements[i] == improvement)
            {
                Debug.Log("Found: " + improvement);
                return true;
            }
        }
        Debug.Log("Found: Nothing");
        return false;
    }

    public void BuildImprovement(string improvement)
    {
        for(int i  = 0; i < improvementCount; i++)
        {
            if(improvements[i] == "")
            {
                improvements[i] = improvement;
                break;
            }
        }
    }
}
