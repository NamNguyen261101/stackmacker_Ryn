using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerMobile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigi;
    [SerializeField] private LayerMask _checkPlayerBridge;
    // speed  
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _target; // player
    // input params
    private Vector3 _mousePosition;
    private Vector3 _input = Vector3.zero;
    private bool _isMoving = false;
    private bool _isStuck = false; // check va cham

    private float dragToMove = 50f; 

    void Start()
    {
        
    }

    
    void Update()
    {
        HandleInputUser();
    }
    /// <summary>
    /// Function (movement): - rigi = movement
    /// </summary>
    private void Move()
    {
        /*if (Input.GetMouseButton(0))
        {
            _mousePosition = Input.mousePosition;

            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(this.transform.position, _mousePosition, step);
        }*/

        _rigi.velocity = _speed * _input;
    }
    /// <summary>
    /// Function: Handle Input from user
    /// </summary>
    /// <returns></returns>
    private void HandleInputUser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var dragVector = Input.mousePosition - _mousePosition;

            if (dragVector.magnitude >= dragToMove)
            {
                dragVector.Normalize();

                // change  vector x - y direction
                if (Mathf.Abs(dragVector.x) >= Mathf.Abs(dragVector.y))
                {
                    dragVector.y = 0;
                } else
                {
                    dragVector.x = 0;
                }
                // Gan lai
                Vector3 newInput = dragVector;
                newInput.z = newInput.y;
                newInput.y = 0;

                // check stuck


                // Moving
                // Gan lai vao vector Input
                _input = newInput;
                _mousePosition = Input.mousePosition;
                Move();

            }
        }
    }
    /// <summary>
    /// Change Route - Direction
    /// </summary>
    private void ChangeDirectionState(DirectionState _direction) // string
    {
        switch (_direction)
        {
            case DirectionState.FORWARD:
                AppleNewRoute(Vector3.forward);
                break;
            case DirectionState.BACKWARD:
                AppleNewRoute(Vector3.back);
                break;
            case DirectionState.LEFT:
                AppleNewRoute(Vector3.left);
                break;
            case DirectionState.RIGHT:
                AppleNewRoute(Vector3.right);
                break;
        }
    }
    /// <summary>
    /// Update Movement after changing state
    /// </summary>
    private void AppleNewRoute(Vector3 newRoute)
    {
        _rigi.velocity = Vector3.zero; // reset 
        _input = newRoute;

        Move();
    }

    /// <summary>
    /// Check Ground when layer touch the brick or not in it
    /// </summary>
    /// <returns></returns>
    // CheckGround
    private bool CheckGround()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + Vector3.down * 1.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, _checkPlayerBridge);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}

public enum DirectionState
{
    FORWARD = 0,
    BACKWARD = 1,
    LEFT = 2,
    RIGHT = 3
}
