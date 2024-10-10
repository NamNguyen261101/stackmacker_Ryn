using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class PlayerController : MonoBehaviour
{
    #region Params
    [SerializeField] private LayerMask _layerMaskToGroundBrick;
    // input
    private Touch _touch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private RaycastHit _hit; // check

    private float _unit = 1f;
    private Vector3 _targetPosition;
    private Vector3 _direction;
    private Vector3 _nextPosition;

    // speed
    [SerializeField] private Direction directionState;
    [SerializeField] private float _speed = 10f;
    private bool _isMoving;
    private bool _isFinish; // finish game

    // Brick
    [SerializeField] private GameObject _containerBrick;
    [SerializeField] private GameObject _containerBrickInBridge;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private float _brickHeight = 0.2f;
    [SerializeField] private int _countBricks = 0;

    private List<GameObject> _listBricks = new List<GameObject>();
    private List<GameObject> _listBricksInBridge = new List<GameObject>();

    // Canvas Count

    #endregion

    void Start()
    {
        _nextPosition = transform.position;
        _countBricks = 0;
        _brickHeight = _brickPrefab.GetComponent<MeshRenderer>().bounds.size.y; // resett lai 
        _isMoving = false;
        _isFinish = false;
        OnInit();
        // CanvasController.Instance.UpdateStackIndicatorText(_listBricks.Count);
    }

    public void OnInit()
    {
        _countBricks = 0;

        ClearBrick();
    }

    void Update()
    {
        CheckDirection();
        if (directionState != Direction.NONE && !_isMoving)
        {
            _nextPosition = TargetPosition(directionState);
            _isMoving = true;
        }

        Move(directionState, _nextPosition);
        if (Vector3.Distance(transform.position, _nextPosition) < 0.1f)
        {
            _isMoving = false;
            directionState = Direction.NONE;
        }
    }
    /// <summary>
    /// Check Direction player controller when touch 
    /// </summary>
    private void CheckDirection()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                _touchStartPosition = _touch.position;
            }
            else if (_touch.phase == TouchPhase.Moved || TouchPhase.Moved == TouchPhase.Ended)
            {
                _touchEndPosition = _touch.position;

                float x = _touchEndPosition.x - _touchStartPosition.x;
                float y = _touchEndPosition.y - _touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    directionState = Direction.NONE;
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    directionState = x > 0 ? Direction.RIGHT : Direction.LEFT;
                   
                }
                else
                {
                    directionState = y > 0 ? Direction.FORWARD : Direction.BACK;
                    
                }
            }
        }
    }
    /// <summary>
    /// Moving - player
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="nextPos"></param>
    private void Move(Direction direction, Vector3 nextPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
    }

    /// <summary>
    /// Take a target position player
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Vector3 TargetPosition(Direction dir)
    {
        _targetPosition = Vector3.zero;
        switch (dir)
        {
            case Direction.FORWARD:
                _direction = Vector3.forward;
                break;

            case Direction.BACK:
                _direction = Vector3.back;
                break;

            case Direction.RIGHT:
                _direction = Vector3.right;
                break;

            case Direction.LEFT:
                _direction = Vector3.left;
                break;
            default:
                return transform.position;
        }
        Vector3 nextPosition = this.transform.position;

        while (Physics.Raycast(nextPosition + _direction * _unit, Vector3.down, out _hit, 5f, _layerMaskToGroundBrick))
        {
            if (_hit.transform.CompareTag("Brick") || _hit.transform.CompareTag("Unbrick") || _hit.transform.CompareTag("Bridge"))
            {
                // Debug.Log(_hit.transform.tag);
                nextPosition = nextPosition + _direction * _unit;
            }
            else 
            {
                break;
            }
        }
        if (Physics.Raycast(nextPosition + _direction * _unit, Vector3.down, out _hit, 5f, _layerMaskToGroundBrick) || _hit.transform.CompareTag("Finished"))
        {
            _isFinish = true;
        }
            /* for (int i = 0; i < 100; i++)
             {
                 if (Physics.Raycast(nextPosition + Vector3.up * 1f, Vector3.down, out _hit, 5f, _layerMaskToGroundBrick))
                 {
                     Debug.DrawRay(nextPosition + Vector3.up * 1f, Vector3.down * 5f, Color.red, 5);
                     // luu lai moi lan ban trung
                     Debug.Log("Did Hit");
                     _targetPosition = _hit.transform.parent.position;

                     nextPosition.z += 1;
                 }
                 else
                 {
                     Debug.DrawRay(nextPosition + Vector3.up * 1f, Vector3.down * 5f, Color.green);
                     Debug.Log("Did not Hit");
                     break;
                 }
             }*/

            /*if (Physics.Raycast(nextPosition + _direction * _unit, Vector3.down, out _hit, 5f) && _hit.transform.CompareTag("Destination"))
            {
                _isFinish = true;
                GameObject finishObject = _hit.transform.gameObject;
                GameObject chestObject = finishObject.transform.Find("baoxiang_close").gameObject;

                chestWidth = chestObject.GetComponent<Collider>().bounds.size.z;
                Vector3 finishPos = chestObject.transform.position - new Vector3(0, 0, chestWidth / 2);
                nextPosition = finishPos;
            }*/
            return nextPosition;
    }

    /// <summary>
    /// Add brick to under player -> When touched and add to brickContainer
    /// </summary>
    /// <param name="newBrick"></param>
    private void AddBrick(GameObject newBrick)
    {
        // add to object
        newBrick = Instantiate(_brickPrefab, transform.position, Quaternion.Euler(-90, 0, -180)); // as _containerBrick
        newBrick.transform.parent = _containerBrick.transform; // 
        newBrick.tag = "Untagged";
        // newBrick.GetComponent<BoxCollider>().enabled = false;
        newBrick.transform.position = new Vector3(
                                                newBrick.transform.position.x,
                                                newBrick.transform.position.y + _brickHeight * _countBricks - 0.5f, 
                                                newBrick.transform.position.z); // 0,3f

        
        _listBricks.Add(newBrick);

       
        _countBricks++;
        GameObject child = transform.Find("jiao").gameObject; // jiao
        if (_countBricks > 0)
        {
            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + _brickHeight, child.transform.position.z);
        }
        // Update UI
        // CanvasController.Instance.UpdateStackIndicatorText(_listBricks.Count);
    }
    /// <summary>
    /// Remove brick when go through Bridge -> UnBrick
    /// </summary>
    private void RemoveBrick(Collider bridge)
    {
        _countBricks--; 
        if (_listBricks.Count > 0)
        {
            Destroy(_listBricks[_listBricks.Count - 1]);
            _listBricks.RemoveAt(_listBricks.Count - 1);
        }


        GameObject child = transform.Find("jiao").gameObject;
        if (_countBricks > 1)
        {
            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y - _brickHeight, child.transform.position.z);
        }

        bridge.gameObject.tag = "Bridge";
        // Update UI
        // CanvasController.Instance.UpdateStackIndicatorText(_listBricks.Count);
    }

    /// <summary>
    /// Clear all brick before start
    /// </summary>
    private void ClearBrick()
    {
        GameObject child = transform.Find("jiao").gameObject;
       
       
        for (int i = 0; i < _listBricks.Count; i++)
        {
            // Debug.Log("Destroy brick");
            Destroy(_listBricks[i].gameObject);
            if (i > 0)
            {
                child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y - _brickHeight, child.transform.position.z);
            }
           
        }

        _listBricks.Clear();
        
    }

    private void HandleWithFinishRace()
    {
        _isFinish = true;
        GameManager.ActionLevelPassed.Invoke();
        ClearBrick();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
           
            if (other.gameObject.GetComponent<Renderer>().enabled)
            {
                
                other.gameObject.GetComponent<Renderer>().enabled = false;
                AddBrick(other.gameObject);
                
            }
        }
        else if (other.gameObject.CompareTag("Unbrick"))
        {
            RemoveBrick(other);
            GameObject brickInBridge = Instantiate(_brickPrefab, other.transform.position, Quaternion.Euler(-90, 0, -180));
            int _countBrickInBridge = 0;

            _countBrickInBridge += 1;

            brickInBridge.tag = "Bridge";
            brickInBridge.transform.parent = _containerBrickInBridge.transform;

            brickInBridge.transform.position = new Vector3(
                                               brickInBridge.transform.position.x,
                                               brickInBridge.transform.position.y + _brickHeight * _countBrickInBridge - 0.3f,
                                               brickInBridge.transform.position.z);
            _listBricksInBridge.Add(brickInBridge);

        }


        if (other.gameObject.name == "zhongdian")
        {
            ClearBrick();
        }
    }

   

    public enum Direction
    {
        NONE,
        FORWARD,
        BACK,
        RIGHT,
        LEFT
    }
}
