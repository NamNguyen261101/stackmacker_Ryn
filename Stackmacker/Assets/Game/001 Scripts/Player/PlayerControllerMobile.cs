using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMobile : MonoBehaviour
{
    private bool _tap = false, 
                _swipeLeft = false, 
                _swipeRight = false, 
                _swipeUp = false, 
                _swipeDown = false;
    [SerializeField] private Vector2 _swipeDelta, _startTouch;
    [SerializeField] private LayerMask _checkPlayerBridge;

    // [SerializeField] private DirectionState _directionState = DirectionState.FORWARD;

    // Try moveToward
    [SerializeField] private float _speed = -10f;
    [SerializeField] private Transform _target; // player
    [SerializeField] private Vector3 _mousePosition;

    void Start()
    {
        
    }

    
    void Update()
    {
        Movemement();
    }
    /// <summary>
    /// Function (movement): Player control by mouse or Input touch
    /// </summary>
    private void Movemement()
    {
        // Input for mouse and touch
        // Vector2.MoveTowards(_currentTarget, 
       /* if (Input.GetMouseButtonDown(0))
        {
            _startTouch = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0))
        {
            _startTouch = _swipeDelta = Vector2.zero;
        }

        _swipeDelta = Vector2.zero; 
        if (_startTouch != Vector2.zero)
        {
            if (Input.touches.Length != 0)
            {
                _swipeDelta = Input.touches[0].position - _startTouch;
            } else if (Input.GetMouseButton(0))
            {
                _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
            }
        }*/

        if (Input.GetMouseButton(0))
        {
            _mousePosition = Input.mousePosition;

            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(this.transform.position, _mousePosition, step);
        }
    }
    // CheckGround
    private bool CheckGround()
    {

        return true;
    }
}

public enum DirectionState
{
    FORWARD = 0,
    BACKWARD = 1,
    LEFT = 2,
    RIGHT = 3
}
