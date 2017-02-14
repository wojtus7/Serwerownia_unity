using CnControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetViewController : MonoBehaviour {


    public float speed = 6.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    private Vector3 startingPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //controller.enabled = false;
        startingPosition = this.transform.position;
        enabled = false;
        GetComponent<Camera>().enabled = false;
    }

    void Update()
    {
        moveDirection = new Vector3(0f, CnInputManager.GetAxis("Vertical"), 0f);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;


        //Debug.Log(transform.localPosition.z);
        if (transform.localPosition.z < 5.0f && moveDirection.z > 0)
            controller.Move(moveDirection * Time.deltaTime);
        if (transform.localPosition.z > 1.5f && moveDirection.z < 0)
            controller.Move(moveDirection * Time.deltaTime);
            
    }

    void OnDisable()
    {
        this.transform.position = startingPosition;
    }
    

}
