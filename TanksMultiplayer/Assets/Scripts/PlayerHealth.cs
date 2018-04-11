using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
	public const int maxHealth = 100;
    [SyncVar]
    public int health = maxHealth;
    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }
        if (health > 0)
        {
            health -= amount;
            Debug.Log("Hero health: " + health);
        }
        else
        {
            Debug.Log("Game Over");
            Destroy(this.gameObject);
        }
    }
	    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            var damage = other.GetComponent<Bullet>().damage;
            TakeDamage(damage);
        }
    }
}
