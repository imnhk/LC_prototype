using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnSpot : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
