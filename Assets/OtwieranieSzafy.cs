using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtwieranieSzafy : MonoBehaviour
{
    int numerSzafy = 1;

    bool hasCollided = false;
    string labelText = "";

    void OnGUI()
    {
        if (hasCollided == true)
        {
            GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), (labelText));
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            hasCollided = true;
            labelText = "Wciśnij E aby zajrzeć do szafy.";
            var pa = c.gameObject.GetComponent<PlayerActions>();
            if (pa != null)
            {
                pa.InCabinetProximity = numerSzafy;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        hasCollided = false;
        var pa = c.gameObject.GetComponent<PlayerActions>();
        if (pa != null)
        {
            pa.InCabinetProximity = 0;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
