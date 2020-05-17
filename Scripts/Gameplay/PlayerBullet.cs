using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticle;

    [SerializeField] private AudioClip bulletOnBodyAudio;
    [SerializeField] private AudioClip bulletOnStoneAudio;

    public int Damage { get; set; }

    [Range(10f, 50f), SerializeField]
    private float speed = 30f;

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
        if (other.tag == "Enemy")
        {
            this.gameObject.SetActive(false);
            other.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(Damage);
            PumpFramework.Common.SoundManager.Instance.PlaySound(bulletOnBodyAudio, this.transform.position, false);

        }
        else if (other.tag == "Obstacle")
        {
            this.gameObject.SetActive(false);
            PumpFramework.Common.SoundManager.Instance.PlaySound(bulletOnStoneAudio, this.transform.position, false);

        }
    }
}
