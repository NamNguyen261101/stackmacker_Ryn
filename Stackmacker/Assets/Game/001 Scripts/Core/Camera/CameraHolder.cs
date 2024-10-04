using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform TargetObject;
    public float MoveSmoothTime = 0.3F;
    public float RotationSpeed = 6f;
    public Vector3 Offset = new Vector3(0f, 15f, 0f);

    public bool Enabled = true;

    private Transform myTransform;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        myTransform = this.transform;
    }

    void Update()
    {
        if (TargetObject != null && Enabled)
        {
            Vector3 targetPosition = TargetObject.TransformPoint(Offset);
            myTransform.position = Vector3.SmoothDamp(myTransform.position, targetPosition, ref velocity, MoveSmoothTime);
            if (RotationSpeed > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(TargetObject.position - myTransform.position);
                this.transform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
            }
        }
    }
}
