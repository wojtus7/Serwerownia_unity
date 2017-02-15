using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetProximityTrigger : MonoBehaviour
{
    public int numerSzafy = 1;
    public int numerDrzwi = 1; //1 - przod; 2 - tyl

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
            labelText = string.Format("Wciśnij E aby zajrzeć do szafy {0}.", numerSzafy);
            var pa = c.gameObject.GetComponent<PlayerActions>();
            if (pa != null)
            {
                pa.InCabinetProximity = numerSzafy;
                pa.CabinetDoorNumber = numerDrzwi;
            }
            //OpenDoor(true);
        }
    }

    void OnTriggerExit(Collider c)
    {
        hasCollided = false;
        var pa = c.gameObject.GetComponent<PlayerActions>();
        if (pa != null)
        {
            pa.InCabinetProximity = 0;
            pa.CabinetDoorNumber = 0;
        }
        //OpenDoor(false);
    }

    private void OpenDoor(bool open)
    {
        var door = transform.Find("szafa_1_Drzwi");

        if(open)
            door.transform.Rotate(0f, 0f, -100f);
        else
            door.transform.Rotate(0f, 0f, 100f);
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Open"))
        //{

        //}
    }
}
