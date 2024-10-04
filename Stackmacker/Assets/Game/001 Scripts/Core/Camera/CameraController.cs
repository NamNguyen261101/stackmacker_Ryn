using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private float _rotationSpeed = 0.125f;
    [SerializeField] private Vector3 _locationOffset;
    [SerializeField] private Vector3 _rotationOffset; // offset truc

    private void Start()
    {
       
    }
    private void Update()
    {
        Vector3 desiredPosition = _target.position + _target.rotation * _locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, _smoothSpeed);
        this.transform.position = smoothedPosition;

        Quaternion desiredRotation = _target.rotation * Quaternion.Euler(_rotationOffset);
        Quaternion smoothedRotation = Quaternion.Lerp(this.transform.rotation, desiredRotation, _rotationSpeed);
        this.transform.rotation = smoothedRotation;
        

    }

    
}
