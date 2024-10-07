using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform TargetObject;
    [SerializeField] private float MoveSmoothTime = 0.3F;
    [SerializeField] private float RotationSpeed = 6f;
    [SerializeField] private Vector3 Offset = new Vector3(0f, 15f, 0f);

    [SerializeField] private bool Enabled = true;

    private Transform myTransform;
    private Vector3 velocity = Vector3.zero;

    #endregion 


    void Awake()
    {
        myTransform = this.transform;
    }

    /// <summary>
    /// Camera follow to target even rotation
    /// </summary>
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
