using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    #region Variables
        [SerializeField] private Rigidbody _rigi;
        [SerializeField] private LayerMask _layerMaskToGroundBrick;
        
        [SerializeField] private SwipeDirection _swipeDirection;        
    // speed
        [SerializeField] private float _speed;

        private bool swiping = false;
        private bool eventSent = false;
        private Vector2 lastPosition;
        private Vector2 _startPosition;

        Vector3 target;
        Vector3 startPosition;

    // brick
        
        [SerializeField] private int defaultBricks = 0;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

   
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0 )
        {
            
            if (swiping == false)
            {
                swiping = true;
                lastPosition = Input.GetTouch(0).position;
                return;
            }
            else
            {
                if (!eventSent)
                {
                    
                    Vector2 direction = Input.GetTouch(0).position - lastPosition;
                    GetTargetPosition(_swipeDirection);

                    //
                    this.transform.position = Vector3.MoveTowards(target, direction, _speed);

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {

                        if (direction.x > 0)
                        {
                            _swipeDirection = SwipeDirection.Right;
                        }
                        else
                        {
                            _swipeDirection = SwipeDirection.Left;  
                        }
                    }
                    else
                    {
                        if (direction.y > 0)
                        {
                            _swipeDirection = SwipeDirection.Up;
                        }    
                        else
                        {
                            _swipeDirection = SwipeDirection.Down;
                        }
                    }

                    eventSent = true;
                    
                }
            }
        }
        else
        {
            swiping = false;
            eventSent = false;
        }
    }

    private Vector3 GetTargetPosition(SwipeDirection swipeDirection)
    {
        target = Vector3.zero;
        startPosition = this.transform.position;
        RaycastHit hit;
        switch (swipeDirection)
        {
            case SwipeDirection.Left:
                startPosition += Vector3.left * 0.75f;
                break;
            case SwipeDirection.Right:
                startPosition += Vector3.right * 0.75f;
                break;
            case SwipeDirection.Up:
                startPosition += Vector3.forward * 0.75f;
                Debug.Log(startPosition);
                break;
            case SwipeDirection.Down:
                startPosition += Vector3.back * 0.75f;
                break;

        }

        for (int i = 0; i < 100; i++)
        {
            if (Physics.Raycast(startPosition + Vector3.up * 1f, Vector3.down, out hit, 5f, _layerMaskToGroundBrick))
            {
                Debug.DrawRay(startPosition + Vector3.up * 1f, Vector3.down * 5f, Color.red, 5);
                // luu lai moi lan ban trung
                Debug.Log("Did Hit");
                target = hit.transform.parent.position;

                startPosition.z += 1;
            }
            else
            {
                Debug.DrawRay(startPosition + Vector3.up * 1f, Vector3.down * 5f, Color.green);
                Debug.Log("Did not Hit");
                break;
            }

            
        }
        return target;
    }

    /// <summary>
    /// Move
    /// </summary>
    private void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, 0.3f));
            if (touch.phase == TouchPhase.Stationary)
            {
                if (touchPos.x < 0)
                    _speed = 8f;
                else if (touchPos.x > 0)
                    _speed = -8f;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _speed = 0f;
            }

            Debug.LogError("touch");
        }
    }




    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }
}
