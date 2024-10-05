using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    
    void Start()
    {
        _nextPosition = transform.position;
        _isMoving = false;
        _isFinish = false;

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
                    Debug.LogError("Left - right");
                }
                else
                {
                    directionState = y > 0 ? Direction.FORWARD : Direction.BACK;
                    Debug.LogError("Back - forward");
                }
            }
        }
    }
    private void Move(Direction direction, Vector3 nextPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
    }


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
                Debug.Log(_hit.transform.tag);
                nextPosition = nextPosition + _direction * _unit;
            }
            else break;
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
    public enum Direction
    {
        NONE,
        FORWARD,
        BACK,
        RIGHT,
        LEFT
    }
}
