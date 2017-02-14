using UnityEngine;
using CnControls;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float RotationSpeed = 15f;

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
        // camera
        //var horizontalMovement = -CnInputManager.GetAxis("CameraHorizontal");
        //var verticalMovement = CnInputManager.GetAxis("CameraVertical");
        //_mainCameraTransform.Rotate(Vector3.up, horizontalMovement * Time.deltaTime * RotationSpeed);
        //_mainCameraTransform.Rotate(Vector3.right, verticalMovement * Time.deltaTime * RotationSpeed);

        //_transform.Rotate(Vector3.up, horizontalMovement * Time.deltaTime * RotationSpeed);
        RotateCamera();

        // movement
        var inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));
        Vector3 movementVector = Vector3.zero;

        // If we have some input
        if (inputVector.sqrMagnitude > 0.001f)
        {
            movementVector = _mainCameraTransform.TransformDirection(inputVector);
            movementVector.y = 0f;
            movementVector.Normalize();
            //_transform.forward = movementVector; // rotate towards move direction
        }

        movementVector += Physics.gravity;
        _characterController.Move(movementVector * Time.deltaTime * MovementSpeed);
    }
    
    private void RotateCamera()
    {
        float yRot = CnInputManager.GetAxis("CameraHorizontal");
        float xRot = CnInputManager.GetAxis("CameraVertical");

        //var m_CharacterTargetRot = _transform.localRotation;
        //var m_CameraTargetRot = _mainCameraTransform.localRotation;
        //m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        //m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        transform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
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
