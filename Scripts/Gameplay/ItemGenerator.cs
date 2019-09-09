using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : PumpFramework.Common.Singleton<ItemGenerator>
{
    private Transform itemSpawnSpots;

    [SerializeField]
    private GameObject medicBoxPrefab;

    private void Awake()
    {
        this.itemSpawnSpots = GameObject.Find("ItemSpawnSpots").GetComponent<Transform>();
    }

    private void Start()
    {
        Transform[] itemSpots = this.itemSpawnSpots.GetComponentsInChildren<Transform>();
        foreach (Transform spot in itemSpots)
        {
            // 부모 오브젝트는 제외한다
            if (spot.name == this.itemSpawnSpots.name)
                continue;

            GameObject item = Instantiate<GameObject>(medicBoxPrefab, spot.position, spot.rotation);
            item.transform.parent = this.transform;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        
    }
}
