using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private Vector2 movement;
    [Range(0.1f, 3f), SerializeField]
    private float duration;
    private float durationCounter;

    void Start()
    {
        movement = new Vector2(Random.Range(-30f, 30f), Random.Range(120f, 150f));
        durationCounter = duration;
    }

    void Update()
    {
        durationCounter -= Time.deltaTime;

        this.transform.Translate(movement * Time.deltaTime);

        if (durationCounter < 0.8f * duration)
        {
            movement.y = movement.y - 8f;
        }

        if (durationCounter < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
