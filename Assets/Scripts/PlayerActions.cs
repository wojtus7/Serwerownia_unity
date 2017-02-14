using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerActions : MonoBehaviour {

    public int InCabinetProximity = 0; // nr szafki
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


                        //var ccc = cabinetCamera.GetComponent<CharacterController>();
                        //ccc.enabled = true;

                        cabinetController = cabinet.GetComponentInChildren<CabinetViewController>();
                        SwitchControl(cabinetController, normalController);

                        isInCabinet = true;
                    }
                }
                else
                {
                    tempCamera.enabled = false;
                    var cam = this.gameObject.GetComponentInChildren<Camera>();
                    this.GetComponentInChildren<Camera>().enabled = true;


                    //this.GetComponent<CharacterController>().enabled = true;
                    SwitchControl(normalController, cabinetController);

                    isInCabinet = false;
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
