using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEffect : MonoBehaviour
{
    public int Damage { get; set; }

    private float createdTime;
    private float duration = 0.5f;

    void Start()
    {
        createdTime = Time.time;
    }

    void Update()
    {
        // 시간이 지나면 자동으로 삭제한다
        if (Time.time > createdTime + duration)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            other.transform.gameObject.GetComponent<Player>().TakeDamage(Damage);
        }
    }
}
