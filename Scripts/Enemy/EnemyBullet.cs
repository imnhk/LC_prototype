using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int Damage { get; set; }

    [SerializeField]
    private float speed = 10f;

    private float createdTime;
    private float duration = 5f;

    private void OnEnable()
    { 
        createdTime = Time.time;
    }

    void Update()
    {
        // 그냥 앞으로 이동
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        // 시간이 지나면 자동으로 삭제한다
        if (Time.time > createdTime + duration)
            this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.gameObject.SetActive(false);
            other.transform.gameObject.GetComponent<Player>().TakeDamage(Damage);
        }
        else if(other.tag == "Obstacle")
        {
            this.gameObject.SetActive(false);
        }
    }

}
