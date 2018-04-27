using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region PUBLIC PROPERTIES
    public float bulletSpeed = 3.0f;
    public AudioClip audioShoot = null;
    public AudioClip audioHit = null;
    #endregion

    #region MonoBehaviour CallBacks
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
        Destroy(this.gameObject, audioHit.length);
    }
    #endregion
}

