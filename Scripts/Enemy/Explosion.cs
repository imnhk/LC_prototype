using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [System.NonSerialized]
    public int damage;
    [System.NonSerialized]
    public float maxRadius;

    private float radius = 0;
    private MeshRenderer sphereRenderer;

    [SerializeField]
    private float expSpeed;
    [SerializeField]
    private AudioClip explosionAudio;


    private void Awake()
    {
        sphereRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        sphereRenderer.material.color = new Color(1, 1, 0.5f, 0.3f);
        sphereRenderer.transform.localScale = Vector3.zero;

        PumpFramework.Common.SoundManager.Instance.PlaySound(explosionAudio, this.transform.position, false);
    }

    // Update is called once per frame
    void Update()
    {
        radius += expSpeed * Time.deltaTime;
        sphereRenderer.transform.localScale = Vector3.one * radius;

        if (radius > maxRadius)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
