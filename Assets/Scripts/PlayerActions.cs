using CnControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerActions : MonoBehaviour {

    public int InCabinetProximity = 0; // nr szafki
    public int CabinetDoorNumber = 0; // 1 - przednie, 2 - tylne
    bool isInCabinetView = false;
    Camera tempCamera;

    PlayerController normalController;
    CabinetViewController cabinetController;

    private GameObject[] szafki = new GameObject[5];
    private GameObject currentCabinet;

    // Use this for initialization
    void Start () {
        normalController = this.GetComponent<PlayerController>();

        szafki[0] = GameObject.Find("szafa_" + 1);
        szafki[1] = GameObject.Find("szafa_" + 2);
        szafki[2] = GameObject.Find("szafa_" + 3);
        szafki[3] = GameObject.Find("szafa_" + 4);
        szafki[4] = GameObject.Find("szafa_" + 5);
    }
	
	// Update is called once per frame
	void Update () {

        if (InCabinetProximity > 0)
        {
            if (CnInputManager.GetButtonDown("Open"))
            {
                if(!isInCabinetView)
                {
                    currentCabinet = szafki[InCabinetProximity - 1];

                    if(CabinetDoorNumber == 1)
                    {
                        var currentCabinetDoor = currentCabinet.transform.Find("szafa_TriggerFront");
                        SwitchToCabinetView(currentCabinetDoor);
                    }
                    else if(CabinetDoorNumber == 2)
                    {
                        var currentCabinetDoor = currentCabinet.transform.Find("szafa_TriggerBack");
                        SwitchToCabinetView(currentCabinetDoor);
                    }

                }
                else
                {
                    //zamykanie
                    OpenDoor(CabinetDoorNumber, false);

                    // zmiana kamery
                    tempCamera.enabled = false;
                    var cam = this.gameObject.GetComponentInChildren<Camera>();
                    this.GetComponentInChildren<Camera>().enabled = true;

                    // wlaczenie coliddera
                    this.GetComponent<CharacterController>().enabled = true;

                    SwitchControl(normalController, cabinetController);

                    isInCabinetView = false;
                }

            }
        }

	}

    private void SwitchToCabinetView(Transform currentCabinetDoor)
    {
        //otwieranie
        OpenDoor(CabinetDoorNumber, true);

        //zmiana kamery
        var cabinetCamera = currentCabinetDoor.GetComponentInChildren<Camera>();
        this.GetComponentInChildren<Camera>().enabled = false;
        tempCamera = cabinetCamera;
        tempCamera.enabled = true;


        // wylaczenie collidera
        GetComponent<CharacterController>().enabled = false;

        // zmiana inputu
        cabinetController = currentCabinetDoor.GetComponentInChildren<CabinetViewController>();
        SwitchControl(cabinetController, normalController);

        isInCabinetView = true;
    }

    private void SwitchToNormalView()
    {

    }

    private void SwitchControl(MonoBehaviour inputToEnable, MonoBehaviour inputToDisable)
    {
        inputToDisable.enabled = false;
        inputToEnable.enabled = true;
    }

    private void OpenDoor(int doorNumber, bool open)
    {
        if (doorNumber == 1)
        {
            var door = currentCabinet.transform.Find("szafa_1_Drzwi");
            if (open)
                door.transform.Rotate(0f, 0f, -100f);
            else
                door.transform.Rotate(0f, 0f, 100f);
        }
        else if (doorNumber == 2)
        {
            var doorL = currentCabinet.transform.Find("szafa_1_D_L");
            var doorP = currentCabinet.transform.Find("szafa_1_D_P");

            if (open)
            {
                doorL.transform.Rotate(0f, 0f, -100f);
                doorP.transform.Rotate(0f, 0f, -100f);
            }   
            else
            {
                doorL.transform.Rotate(0f, 0f, 100f);
                doorP.transform.Rotate(0f, 0f, -100f);
            }

        }
       


    }
}
