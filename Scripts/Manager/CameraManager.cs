using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float smoothing = 5f;

    [SerializeField]
    private Transform target;    
    private Vector3 targetOffset;
  
    void Start()
    {
        this.target = PlayerControl.Instance.transform.GetComponentInChildren<Player>().transform;
        this.transform.position = this.target.position + new Vector3(0, 16, -16);
        this.targetOffset = this.transform.position - this.target.position;   
    }

    void FixedUpdate()
    {
        Vector3 targetCameraPosition = this.target.position + this.targetOffset;
        this.transform.position = Vector3.Lerp(this.transform.position, targetCameraPosition, smoothing * Time.deltaTime);
    }
}
