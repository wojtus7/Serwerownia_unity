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
    private GameObject currentOpenCabinet;

    private GameObject glassDoor;
    private bool glassDoorOpened = false;
    private int currentDoorNumber = 0;

    // Use this for initialization
    void Start () {
        normalController = this.GetComponent<PlayerController>();

        szafki[0] = GameObject.Find("szafa_" + 1);
        szafki[1] = GameObject.Find("szafa_" + 2);
        szafki[2] = GameObject.Find("szafa_" + 3);
        szafki[3] = GameObject.Find("szafa_" + 4);
        szafki[4] = GameObject.Find("szafa_" + 5);

        glassDoor = GameObject.Find("drzwi_szklane");
    }

    // Update is called once per frame
    void Update()
    {

        if (InCabinetProximity > 0)
        {
            if (CnInputManager.GetButtonDown("Open"))
            {
                if (!isInCabinetView)
                {
                    currentCabinet = szafki[InCabinetProximity - 1];
                    currentOpenCabinet = szafki[InCabinetProximity - 1];

                    if (CabinetDoorNumber == 1)
                    {
                        var currentCabinetDoor = currentCabinet.transform.Find("szafa_TriggerFront");
                        SwitchToCabinetView(currentCabinetDoor);
                    }
                    else if (CabinetDoorNumber == 2)
                    {
                        var currentCabinetDoor = currentCabinet.transform.Find("szafa_TriggerBack");
                        SwitchToCabinetView(currentCabinetDoor);
                    }

                }
                else
                {
                    //zamykanie
                    currentCabinet = null;

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
        else if (InCabinetProximity == -1)
        {
            if (CnInputManager.GetButtonDown("Open"))
            {
                var door = glassDoor.transform;
                if (!glassDoorOpened)
                {
                    glassDoorOpened = true;
                }  
                else
                {
                    glassDoorOpened = false;
                }
            }
        }
        MoveDoors(glassDoorOpened);
        MoveServerDoors();
    }

    private void SwitchToCabinetView(Transform currentCabinetDoor)
    {
        //otwieranie
        currentDoorNumber = CabinetDoorNumber;

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

    private void MoveDoors(bool isDoorOpen) {
         if (isDoorOpen) {
            var door = glassDoor.transform;
            if (door.transform.position.x > -6.5) {
                var dir = door.TransformDirection(new Vector3(-0.07f, 0f, 0f));
                door.Translate(dir);
            } else if (door.transform.position.z < 8) {
                var dir = door.TransformDirection(new Vector3(0f, 0f, -0.07f));
                door.Translate(dir);
            }
        } else if (!isDoorOpen) {
            var door = glassDoor.transform;
            if (door.transform.position.z > 5.0) {
                var dir = door.TransformDirection(new Vector3(0f, 0f, 0.07f));
                door.Translate(dir);
            } else if (door.transform.position.x < -5.5) {
                var dir = door.TransformDirection(new Vector3(0.07f, 0f, 0f));
                door.Translate(dir);
            }
        }
    }

    private void MoveServerDoors() {
        if (currentDoorNumber == 1) {
            if (currentCabinet && currentOpenCabinet.transform.Find("szafa_1_Drzwi").transform.rotation.y > -0.5) {
                var door = currentCabinet.transform.Find("szafa_1_Drzwi");
                door.transform.Rotate(0f, 0f, -2f);
            }
            else if (!currentCabinet && currentOpenCabinet && currentOpenCabinet.transform.Find("szafa_1_Drzwi").transform.rotation.y <= -0.01) {
                var doorC = currentOpenCabinet.transform.Find("szafa_1_Drzwi");
                doorC.transform.Rotate(0f, 0f, 2f);
            }
        } else if (currentDoorNumber == 2) {
            if (currentCabinet && currentCabinet.transform.Find("szafa_1_D_L").transform.rotation.y < 0.5) {
                var doorL = currentCabinet.transform.Find("szafa_1_D_L");
                doorL.transform.Rotate(0f, 0f, 2f);
                var doorP = currentCabinet.transform.Find("szafa_1_D_P");
                doorP.transform.Rotate(0f, 0f, -2f);
            } else if (!currentCabinet && currentOpenCabinet && currentOpenCabinet.transform.Find("szafa_1_D_L").transform.rotation.y >= 0.01) {
                var doorL = currentOpenCabinet.transform.Find("szafa_1_D_L");
                doorL.transform.Rotate(0f, 0f, -2f);
                var doorP = currentOpenCabinet.transform.Find("szafa_1_D_P");
                doorP.transform.Rotate(0f, 0f, 2f);
            }
        }
    }
}
