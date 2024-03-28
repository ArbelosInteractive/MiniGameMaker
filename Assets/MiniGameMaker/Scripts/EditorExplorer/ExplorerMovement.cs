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
            
            Debug.Log($"MovementDir: {movementDir}");
            if (movementDir.magnitude >= 0.1f)
            {
                // Calculate the movement direction based on the input values
                Vector3 moveDirection = new Vector3(movementDir.x, movementDir.y, movementDir.z);

                // Rotate the movement direction to match the object's rotation
                moveDirection = transform.TransformDirection(moveDirection);

                // Apply the movement to the object
                transform.position += moveDirection.normalized * explorerSpeed * Time.deltaTime;
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
