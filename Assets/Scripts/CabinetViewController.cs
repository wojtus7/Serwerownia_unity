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
        startingPosition = this.transform.position;
    }

    void Update()
    {
        moveDirection = new Vector3(0, CnInputManager.GetAxis("Vertical"), 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        // Apply gravity manually.
        //moveDirection.y -= gravity * Time.deltaTime;
        // Move Character Controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnDisable()
    {
        this.transform.position = startingPosition;
    }
    

}
