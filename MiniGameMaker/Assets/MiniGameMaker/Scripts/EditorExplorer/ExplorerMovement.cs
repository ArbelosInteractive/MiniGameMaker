using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerMovement : MonoBehaviour
{
    private PlayerInput explorerInputAction;
    private InputAction moveAction;
    private InputAction enableExplorerAction;
    
    [SerializeField] private float explorerSpeed;
    [SerializeField] private float sensitivity;
    
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField]
    private Camera explorerCamera;

    [SerializeField] private CinemachineFreeLook cmFreelook;

    private bool canMove = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        explorerInputAction = GetComponent<PlayerInput>();
        moveAction = explorerInputAction.actions.FindAction("Move");
        enableExplorerAction = explorerInputAction.actions.FindAction("EnableExplorer");
        enableExplorerAction.performed += DisableMovement;
        enableExplorerAction.started += EnableMovement;
        cmFreelook.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateExplorer();
        MoveExplorer();
    }


    void EnableMovement(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Holding Right Mouse Button Started");
        canMove = true;
        cmFreelook.enabled = true;
        Cursor.visible = false;
    }


    void DisableMovement(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Holding Right Mouse Button Ended");
        canMove = false;
        cmFreelook.enabled = false;
        Cursor.visible = true;
    }

    void MoveExplorer()
    {
        if (canMove)
        {
            Vector3 movementDir = moveAction.ReadValue<Vector3>();
            
            if (movementDir.magnitude >= 0.1f)
            {
                // Get the forward and right directions of the object
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                // Flatten the directions so the object doesn't move up or down
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                // Calculate the movement direction based on the input values and the object's rotation
                Vector3 moveDirection = (forward * movementDir.y + right * movementDir.x).normalized;

                // Apply the movement to the object
                transform.position += moveDirection * explorerSpeed * Time.deltaTime;
            }
        }
    }

    void RotateExplorer()
    {
        if (canMove)
        {
            float xAxis = cmFreelook.m_XAxis.m_InputAxisValue;
            float yAxis = cmFreelook.m_YAxis.m_InputAxisValue;

            //Debug.Log($"CM XAxis: {xAxis} , CM YAxis: {yAxis}");

            if (xAxis != 0 | yAxis != 0)
            {
                // Get the rotation of the camera
                Quaternion cameraRotation = explorerCamera.transform.rotation;

                // Set the rotation of the object to match the camera's rotation
                transform.rotation = cameraRotation;
            }
        }
    }

}
