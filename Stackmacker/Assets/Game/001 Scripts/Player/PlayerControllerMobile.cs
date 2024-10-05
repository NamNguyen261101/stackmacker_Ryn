using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControllerMobile : MonoBehaviour
{
    #region Variables
    [SerializeField] private Rigidbody _rigi;
    [SerializeField] private LayerMask _layerMaskToGroundBrick;
    public DirectionState _directionState = DirectionState.FORWARD;
    // speed  
    [SerializeField] private float _speed = 350;
    [SerializeField] private GameObject _target; // player
    private Vector3 _targetPosition;
    // input params
    private Vector3 _mousePosition;
    private Vector3 _input = Vector3.zero;

    private int playerDirection;
    private bool isMoving;
    private bool eventSent = false;

    // Game Manager
    private Vector3 startSwipe,
                    endSwipe;
    
    // Bricks
    List<GameObject> _listBricks = new List<GameObject>();
    [SerializeField] private float _brickHeight = 0.3f;
    [SerializeField] private int _defaultBrick = 0;
    #endregion

    void Start()
    {
        OnInit(); 
    }
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        {
            if (isMoving == false)
            {
                isMoving = true;
                endSwipe = Input.GetTouch(0).position;
                return;
            }

            // Make sure that player swipe after moving
            if (Mathf.Abs(startSwipe.z - (-1)) < 0.05f)
            {
                return;
            }
            // Mouse Down
            

            if (!eventSent)
            {
                endSwipe = Input.GetTouch(0).position;
                // Vector3 direction = Input.GetTouch(0).position - endSwipe;
                GetTargetPosition(_directionState);

                // Horizontal
                if (Mathf.Abs(startSwipe.x - endSwipe.x) > Mathf.Abs(startSwipe.y - endSwipe.y))
                {
                    if (endSwipe.x > startSwipe.x)
                    {
                        // Go North
                        SetDirection((int)DirectionState.FORWARD);
                        Debug.Log("U");
                    }
                    else
                    {
                        SetDirection((int)DirectionState.BACKWARD);
                        Debug.Log("B");
                    }
                }

                // Vertical
                else
                {
                    // Don't do anything, just keep current directioin
                    if (endSwipe.y > startSwipe.y)
                    {
                        SetDirection((int)DirectionState.LEFT);
                        Debug.Log("LEft");
                    }
                    else
                    {
                        SetDirection((int)DirectionState.RIGHT);
                        Debug.Log("R");
                    }
                }

                startSwipe = new Vector3(0, 0, -1);
                // Move
                SetMove(true);
            }       
        }
    }

    public void OnInit()
    {
        startSwipe = new Vector3(0, 0, -1);
        endSwipe = new Vector3(0, 0, -1);

    }
    /// <summary>
    /// Function (movement): - rigi = movement
    /// </summary>
    private void Move()
    {
        _rigi.velocity = _speed * _input;
    }

    private void SetDirection(int direction) 
    {
       playerDirection = direction;
    }

    private void SetMove(bool move)
    {
        this.isMoving = move;
    }

    private Vector3 GetTargetPosition(DirectionState swipeDirection)
    {
        _targetPosition = Vector3.zero;
        startSwipe = this.transform.position;
        RaycastHit hit;
        switch (swipeDirection)
        {
            case DirectionState.LEFT:
                startSwipe += Vector3.left * 0.75f;
                break;
            case DirectionState.RIGHT:
                startSwipe += Vector3.right * 0.75f;
                break;
            case DirectionState.FORWARD:
                startSwipe += Vector3.forward * 0.75f;
                Debug.Log(startSwipe);
                break;
            case DirectionState.BACKWARD:
                startSwipe += Vector3.back * 0.75f;
                break;

        }

        for (int i = 0; i < 100; i++)
        {
            if (Physics.Raycast(startSwipe + Vector3.up * 1f, Vector3.down, out hit, 5f, _layerMaskToGroundBrick))
            {
                Debug.DrawRay(startSwipe + Vector3.up * 1f, Vector3.down * 5f, Color.red, 5);
                // luu lai moi lan ban trung
                Debug.Log("Did Hit");
                _targetPosition = hit.transform.parent.position;

                startSwipe.z += 1;
            }
            else
            {
                Debug.DrawRay(startSwipe + Vector3.up * 1f, Vector3.down * 5f, Color.green);
                Debug.Log("Did not Hit");
                break;
            }
        }
        return _targetPosition;
    }

    /// <summary>
    /// Check Ground when layer touch the brick or not in it
    /// </summary>
    /// <returns></returns>
    // CheckGround
    private bool CheckGround()
    {
        return false;
    }

    public enum DirectionState
    {
        FORWARD = 0,
        BACKWARD = 2,
        LEFT = 3,
        RIGHT = 1
    }

}


