using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerActions : MonoBehaviour {

    public int InCabinetProximity = 0;
    bool isInCabinet = false;
    Camera tempCamera;

    PlayerController normalController;
    CabinetViewController cabinetController;

	// Use this for initialization
	void Start () {
        normalController = this.GetComponent<PlayerController>();
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
                        this.GetComponentInChildren<Camera>().enabled = false;
                        tempCamera = cabinetCamera;
                        tempCamera.enabled = true;
                        isInCabinet = true;
                        cabinetController = cabinet.GetComponentInChildren<CabinetViewController>();
                        this.GetComponent<PlayerController>().enabled = false;
                        SwitchControl(cabinetController, normalController);
                    }
                }
                else
                {
                    tempCamera.enabled = false;
                    var cam = this.gameObject.GetComponentInChildren<Camera>();
                    this.GetComponentInChildren<Camera>().enabled = true;
                    isInCabinet = false;
                    this.GetComponent<PlayerController>().enabled = true;
                    SwitchControl(normalController, cabinetController);
                }

            }
        }

	}

    private void SwitchControl(MonoBehaviour inputToEnable, MonoBehaviour inputToDisable)
    {
        inputToDisable.enabled = false;
        inputToEnable.enabled = true;
    }
}
