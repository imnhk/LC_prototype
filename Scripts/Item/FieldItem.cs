using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Sprite sprite;
    public GameObject prefab;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }
}