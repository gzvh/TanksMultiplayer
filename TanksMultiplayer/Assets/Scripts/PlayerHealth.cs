using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public Text healthText = null;

    void Start()
    {
        healthText = GameObject.Find("healthText").GetComponent<Text>();
        SetHealthText();
    }
    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log("Hero health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player is dead!");
            Destroy(this.gameObject);
        }
    }
    void SetHealthText()
    {
        if (isLocalPlayer)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
    }
    void OnChangeHealth(int health)
    {
        currentHealth = health;
        SetHealthText();
    }
}
