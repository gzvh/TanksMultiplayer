using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{

    public int damage = 10;
    public float bulletSpeed = 3.0f;
    //public Vector3 velocity;
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

    //transform.Translate(0, -bulletSpeed * Time.deltaTime, 0);
    //transform.position += velocity * Time.deltaTime * bulletSpeed;

    void OnTriggerEnter2D(Collider2D other)
    {

        this.GetComponent<AudioSource>().PlayOneShot(audioHit);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, audioHit.length);
    }
}
