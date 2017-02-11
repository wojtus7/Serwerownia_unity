using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtwieranieSzafy : MonoBehaviour
{

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
        }
    }

    void OnTriggerExit(Collider other)
    {
        hasCollided = false;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
