using UnityEngine;
using CnControls;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float RotationSpeed = 0.5f;

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
        float yRot = -CnInputManager.GetAxis("CameraHorizontal") * RotationSpeed;
        float xRot = -CnInputManager.GetAxis("CameraVertical") * RotationSpeed;


        transform.localRotation *= Quaternion.Euler(0f, yRot, 0f) ;
        _mainCameraTransform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);

        //if (clampVerticalRotation)
        //    m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        //if (smooth)
        //{
        //    character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
        //        smoothTime * Time.deltaTime);
        //    camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
        //        smoothTime * Time.deltaTime);
        //}
        //else
        //{
        //    character.localRotation = m_CharacterTargetRot;
        //    camera.localRotation = m_CameraTargetRot;
        //}
    }
}
