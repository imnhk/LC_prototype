using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private Transform itemSpawnSpots;
    [SerializeField]
    private Transform enemySpawnSpots;
    [SerializeField]
    private Transform obstacles;

    [SerializeField]
    private GameObject itemSpawnSpot;
    [SerializeField]
    private GameObject enemySpawnSpot;

    public void AddItemSpot()
    {
        GameObject go = Instantiate(this.itemSpawnSpot, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(this.itemSpawnSpots);
    }

    public void AddEnemySpot()
    {
        GameObject go = Instantiate(this.enemySpawnSpot, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(this.enemySpawnSpots);
    }

    public void CreateMap()
    {
        GameObject go = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);
        go.name = "New Map";

        ItemSpawnSpot[] itemSpots = go.transform.GetComponentsInChildren<ItemSpawnSpot>();
        EnemySpawnSpot[] enemySpots = go.transform.GetComponentsInChildren<EnemySpawnSpot>();

        for (var i = 0; i < itemSpots.Length; ++i)
        {
            Destroy(itemSpots[i]);
        }

        for (var i = 0; i < enemySpots.Length; ++i)
        {
            Destroy(enemySpots[i]);
        }

        Destroy(go.GetComponent<MapMaker>());
    }
}
