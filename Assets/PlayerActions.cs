using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    public int InCabinetProximity = 0;
    bool isInCabinet = false;
    Camera tempCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (InCabinetProximity > 0)
        {
            if (Input.GetButtonDown("Open"))
            {
                if(!isInCabinet)
                {
                    var cabinet = GameObject.Find("szafa_" + InCabinetProximity);

                    var cabinetCamera = cabinet.GetComponentInChildren<Camera>();

                    if (cabinetCamera != null)
                    {
                        this.gameObject.GetComponentInChildren<Camera>().enabled = false;
                        tempCamera = cabinetCamera;
                        tempCamera.enabled = true;
                        isInCabinet = true;
                    }
                }
                else
                {
                    tempCamera.enabled = false;
                    this.gameObject.GetComponentInChildren<Camera>().enabled = true;
                    isInCabinet = false;
                }

            }
        }

	}
}
