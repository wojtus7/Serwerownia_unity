using UnityEngine;
using CnControls;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 4f;
    public float RotationSpeedX = 0.001f;
    public float RotationSpeedY = 0.005f;
    public float MinimumY = -60F;
    public float MaximumY = 60F;
    float RotationY = 0F;

    private Transform _mainCameraTransform;
    private Transform _transform;
    private CharacterController _characterController;

    private void OnEnable()
    {
        _mainCameraTransform = Camera.main.GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
    }

    public void Update()
    {
        RotateCamera();
        
        // movement
        var inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"), 0, CnInputManager.GetAxis("Vertical"));
        Vector3 movementVector = Vector3.zero;

        // If we have some input
        if (inputVector.sqrMagnitude > 0.001f)
        {
            //Debug.Log("inputVector: " + inputVector);
            movementVector = _transform.TransformDirection(inputVector);
            
            movementVector.y = 0f;
            movementVector.Normalize();
            //_transform.forward = movementVector; // rotate towards move direction
            //Debug.Log("Movement Vector: " + movementVector);
        }


        movementVector += Physics.gravity;
        _characterController.Move(movementVector * Time.deltaTime * MovementSpeed);
    }
    
    private void RotateCamera()
    {
        float xRot = -CnInputManager.GetAxis("CameraHorizontal") * RotationSpeedX;
        RotationY += -CnInputManager.GetAxis("CameraVertical") * RotationSpeedY;
        RotationY = Mathf.Clamp(RotationY, MinimumY, MaximumY);

        transform.localRotation *= Quaternion.Euler(0f, xRot, 0f) ;
        _mainCameraTransform.localEulerAngles = new Vector3(-RotationY, transform.localEulerAngles.y, 0);
    }
}
