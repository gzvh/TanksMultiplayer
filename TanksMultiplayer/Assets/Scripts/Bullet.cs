using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{

    public int bulletDamage = 10;
    public float bulletSpeed = 3.0f;
    public AudioClip audioShoot = null;
    public AudioClip audioHit = null;

    void Awake()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioShoot);
    }
    void Update()
    {
        this.transform.Translate(0, -bulletSpeed * Time.deltaTime, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioHit);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;

        var hit = other.gameObject;
        var health = hit.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        Destroy(this.gameObject, audioHit.length);
    }
}
