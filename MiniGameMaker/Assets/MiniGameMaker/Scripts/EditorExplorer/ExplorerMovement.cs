using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerMovement : MonoBehaviour
{
    private PlayerInput explorerInputAction;
    private InputAction moveAction;
    [SerializeField] private float explorerSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        explorerInputAction = GetComponent<PlayerInput>();
        moveAction = explorerInputAction.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        MoveExplorer();   
    }

    void MoveExplorer()
    {
        Vector3 movementDir = moveAction.ReadValue<Vector3>();
        transform.position += new Vector3(movementDir.x, movementDir.y, movementDir.z)  * explorerSpeed * Time.deltaTime;
    }
}
