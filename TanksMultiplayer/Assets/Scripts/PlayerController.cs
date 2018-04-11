using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public float movementSpeed = 1.5f;
    public AudioClip audioDriving = null;
    public GameObject bulletPrefab = null;
    public Transform socket = null;
    private bool shootCooldown = false;

    public override void OnStartLocalPlayer()
    {
        tag = "Player";
    }
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Move();

        if (Input.GetButtonDown("Fire1"))
        {
            if (shootCooldown == false)
            {
                Cmd_Shoot();
                Invoke("ResetCooldown", 0.8f);
                shootCooldown = true;
            }
        }

    }

    private void GetMovement()
    {
        transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
        AudioSource.PlayClipAtPoint(audioDriving, transform.position, 1.0f);
    }
    private void Move()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
        {
            transform.localRotation = Quaternion.Euler(180, 0, 0);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            GetMovement();
            return;
        }
    }
    private void ResetCooldown()
    {
        shootCooldown = false;
    }
    [Command]
    public void Cmd_Shoot()
    {
        GameObject obj = Instantiate(bulletPrefab, socket.position, socket.rotation) as GameObject;
        NetworkServer.Spawn(obj);
    }
}
