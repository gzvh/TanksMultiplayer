using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public PlayerHealth Tank = null;
    public Text PlayerHealth = null;

    void Update()
    {
        DisplayProperties();
    }
    void DisplayProperties()
    {
        PlayerHealth.text = "Health: " + Tank.health.ToString();
    }
}
