using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : PumpFramework.Common.Singleton<NPCGenerator>
{
    private Transform enemySpawnSpots;
    [SerializeField] private List<GameObject> enemyPrefabs = null;
    [SerializeField] private List<EnemyBase> enemies = new List<EnemyBase>();
    public List<EnemyBase> Enemies { get { return this.enemies; } }

    void Awake()
    {
        // 적 생성 스팟 부모를 검색(항상 존재)
        this.enemySpawnSpots = GameObject.Find("EnemySpawnSpots").GetComponent<Transform>();
    }

    private void Start()
    {
        // 적 생성 스팟 하위에 지점마다 적을 생성함
        Transform[] spawnSpots = this.enemySpawnSpots.GetComponentsInChildren<Transform>();
        foreach (Transform spot in spawnSpots)
        {
            // 부모 오브젝트도 spot에 포함되어서 적이 하나 더 생성되는 문제 해결
            if (spot.name == this.enemySpawnSpots.name)
                continue;

            // List에서 무작위 프리팹을 고른다
            GameObject prefab = this.enemyPrefabs[Random.Range(0, this.enemyPrefabs.Count)];
            GameObject enemy = Instantiate<GameObject>(prefab, spot.position, spot.rotation);
            this.enemies.Add(enemy.GetComponent<EnemyBase>());
            enemy.transform.parent = this.transform;
        }
    }

    public EnemyBase GetClosestEnemyFrom(Vector3 pos)
    {
        if (this.enemies.Count <= 0)
        {
            return null;
        }            

        int index = 0;
        float minDistance = float.MaxValue;
        for (var i = 0; i < this.enemies.Count; i++)
        {
            float dist = Vector3.Distance(pos, this.enemies[i].transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                index = i;
            }
        }
        return this.enemies[index];
    }
}
