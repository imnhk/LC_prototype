using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int damage;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float explosionRadius = 8f;

    [System.NonSerialized]
    public Vector3 targetPos;
    private Vector3 startPos;
    private Vector3 movement;

    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private AudioClip throwAudio;

    private void Start()
    {
        startPos = transform.position;
        Vector3 dist = targetPos - startPos;

        PumpFramework.Common.SoundManager.Instance.PlaySound(throwAudio, this.transform.position, false);

        // 초기 y축 속도가 2, 중력가속도가 2이므로 수류탄의 체공시간은 2초.
        // 수류탄이 targetPos에 떨어지게 하려면 초당 1/2만큼 이동해야 한다.
        // 그런데 왜 0.05지
        movement = new Vector3(dist.x * 0.05f, 2, dist.z * 0.05f);
    }

    void Update()
    {

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(2f, 3f, 1f));

        // 중력가속도
        movement.y -= Time.deltaTime * 2;

        if (this.transform.position.y < 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        Explosion exp = Instantiate(explosion, transform.position, transform.rotation).GetComponent<Explosion>();
        exp.damage = this.damage;
        exp.maxRadius = this.explosionRadius;

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            Explode();
        }
    }
}
