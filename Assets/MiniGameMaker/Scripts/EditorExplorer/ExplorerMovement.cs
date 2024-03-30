using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Il2Cpp;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerMovement : MonoBehaviour
{
    private PlayerInput explorerInputAction;
    private InputAction moveAction;
    private InputAction enableExplorerAction;
    private InputAction zoomAction;
    
    [SerializeField] private float explorerSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private CharacterController controller;

    [SerializeField]
    private Camera explorerCamera;

    [SerializeField] private CinemachineFreeLook cmFreelook;

    private bool canMove = false;
    private float zoomValue = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {

        explorerInputAction = GetComponent<PlayerInput>();

        moveAction = explorerInputAction.actions.FindAction("Move");
        enableExplorerAction = explorerInputAction.actions.FindAction("EnableExplorer");
        zoomAction = explorerInputAction.actions.FindAction("Zoom");

        enableExplorerAction.performed += DisableMovement;
        enableExplorerAction.started += EnableMovement;
        zoomAction.performed += ReadZoomValue;

        cmFreelook.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateExplorer();
        MoveExplorer();
        HandleCameraZoom();
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
                controller.Move(moveDirection.normalized * explorerSpeed * Time.deltaTime);
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

    void HandleCameraZoom()
    {
        if(canMove)
        {
            //Can only zoom if not moving
            if(zoomValue > 0)
            {
                Debug.Log("Scrolling Up");
                cmFreelook.m_Orbits[0].m_Radius -= 1f;
                cmFreelook.m_Orbits[1].m_Radius -= 1f;
                cmFreelook.m_Orbits[2].m_Radius -= 1f;
            }
            else if(zoomValue < 0)
            {
                Debug.Log("Scrolling Down");
                cmFreelook.m_Orbits[0].m_Radius += 1f;
                cmFreelook.m_Orbits[1].m_Radius += 1f;
                cmFreelook.m_Orbits[2].m_Radius += 1f;
            }
        }
    }

    void ReadZoomValue(InputAction.CallbackContext callbackContext)
    {
        zoomValue = callbackContext.ReadValue<float>();
    }
}
