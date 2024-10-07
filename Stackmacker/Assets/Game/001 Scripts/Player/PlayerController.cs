using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Params
    [SerializeField] private LayerMask _layerMaskToGroundBrick;
    private Touch _touch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private RaycastHit _hit;

    private float _unit = 1f;
    private Vector3 _targetPosition;
    private Vector3 _direction;
    private Vector3 _nextPosition;

    // speed
    [SerializeField] private Direction directionState;
    [SerializeField] private float _speed = 10f;
    private bool _isMoving;
    private bool _isFinish;

    // Brick
    [SerializeField] private GameObject _containerBrick;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private float _brickHeight = 0.3f;
    [SerializeField] private int _countBricks = 0;

    private List<GameObject> _listBricks = new List<GameObject>();

    #endregion

    void Start()
    {
        _nextPosition = transform.position;
        _isMoving = false;
        _isFinish = false;

        
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
                   // Debug.LogError("Left - right");
                }
                else
                {
                    directionState = y > 0 ? Direction.FORWARD : Direction.BACK;
                    // Debug.LogError("Back - forward");
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
            if (_hit.transform.CompareTag("Brick") || _hit.transform.CompareTag("Unbrick"))
            {
                // Debug.Log(_hit.transform.tag);
                nextPosition = nextPosition + _direction * _unit;
            }
            else 
            {
                break;
            }
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


        return nextPosition;
    }

    /// <summary>
    /// Add brick to under player -> When touched and add to brickContainer
    /// </summary>
    /// <param name="newBrick"></param>
    private void AddBrick(GameObject newBrick)
    {
        // add to object
        newBrick = Instantiate(_brickPrefab, transform.position, Quaternion.Euler(-90, 0, -180));
        // Instantiate(_brickPrefab, transform.position, Quaternion.Euler(-90, 0, -180), _containerBrick.transform);
        // Instantiate (m_Prefab, position, rotation) as GameObject).transform.parent = parentGameObject.transform
        newBrick.transform.parent = transform;
        newBrick.tag = "Untagged";
        newBrick.GetComponent<BoxCollider>().enabled = false;
        newBrick.transform.position = new Vector3(
                                                newBrick.transform.position.x,
                                                newBrick.transform.position.y + _brickHeight * _countBricks - 0.3f,
                                                newBrick.transform.position.z);
        _listBricks.Add(newBrick);

        _countBricks++;
        // Debug.Log("da add thanh cong");
        GameObject child = transform.Find("jiao").gameObject; // jiao
        //GameObject child2 = _containerBrick.transform.GetChild(3).gameObject;
        if (_countBricks > 0)
        {

            child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y + _brickHeight, child.transform.position.z);
            //child2.transform.position = new Vector3(child2.transform.position.x, child2.transform.position.y + _brickHeight, child2.transform.position.z);
        }
    }

    /*private void AddBrick(GameObject newBrick)
    {
        newBrick = Instantiate(_brickPrefab,
                               new Vector3(0, _listBricks.Count * _brickHeight, 0) + transform.position,
                               Quaternion.Euler(-90, 0, -180),_containerBrick.transform);

        newBrick.transform.parent = transform;
        newBrick.tag = "Untagged";
        newBrick.GetComponent<BoxCollider>().enabled = false;
        _listBricks.Add(newBrick);
        
        newBrick.transform.position = (new Vector3(0, (_listBricks.Count - 1) * _brickHeight, 0)) + transform.position;
    }*/

    /// <summary>
    /// Remove brick when go through Bridge -> UnBrick
    /// </summary>
    private void RemoveBrick()
    {
        _countBricks--;
        Destroy(_listBricks[_listBricks.Count - 1]);
        _listBricks.RemoveAt(_listBricks.Count - 1);
        this.transform.position = (new Vector3(0, (_listBricks.Count - 1) * _brickHeight, 0)) + transform.position;
    }
    /// <summary>
    /// Clear all brick before start
    /// </summary>
    private void ClearBrick()
    {
        while (_listBricks.Count >0)
        {
            RemoveBrick();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            if (other.gameObject.GetComponent<Renderer>().enabled)
            {
                other.gameObject.GetComponent<Renderer>().enabled = false;
                AddBrick(other.gameObject);
                // AddBrick();
            }
        }
    }
    /*void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("UnBrick"))
        {
            RemoveBrick();
            Instantiate(_brickPrefab, collisionInfo.transform.position, Quaternion.Euler(-90, 0, -180));
        }
        Debug.Log(_countBricks);
    }*/
    public enum Direction
    {
        NONE,
        FORWARD,
        BACK,
        RIGHT,
        LEFT
    }
}
