using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartSpot : MonoBehaviour
{
    void OnDrawGizmos()
    {       
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
