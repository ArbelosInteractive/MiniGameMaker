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
                Vector3 moveDirection = CalculateMovement(movementDir);
                transform.position += moveDirection * explorerSpeed * Time.deltaTime;
            }
        }
    }

    void RotateExplorer()
    {
        if (canMove)
        {
            //Debug.Log($"{Input.GetAxis(cmFreelook.m_YAxis.m_InputAxisName)},{Input.GetAxis(cmFreelook.m_XAxis.m_InputAxisName)}");
            Vector3 cameraInput = new Vector3(Input.GetAxis(cmFreelook.m_YAxis.m_InputAxisName),
                Input.GetAxis(cmFreelook.m_XAxis.m_InputAxisName), 0);

            transform.Rotate(cameraInput * sensitivity * Time.deltaTime * 50);
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
            
        }
    }

    private Vector3 CalculateMovement(Vector3 inputDir)
    {
        // Get the forward and right vectors of the camera
        Vector3 cameraForward = explorerCamera.transform.forward;
        Vector3 cameraRight = explorerCamera.transform.right;

        // Flatten the vectors so the player doesn't move up or down
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize the vectors to avoid faster movement diagonally
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the move direction based on camera orientation
        Vector3 moveDirection = cameraForward * inputDir.z + cameraRight * inputDir.x;
        return moveDirection.normalized;
    }
}
