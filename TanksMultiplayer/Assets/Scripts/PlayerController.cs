using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public float movementSpeed = 1.5f;
    //public float bulletSpeed = 3.0f;
    public AudioClip audioDriving = null;
    public GameObject bulletPrefab = null;
    public Transform socket = null;
    private float shootCooldown = 0.8f;
    private float shootTimer = 0f;

    public override void OnStartLocalPlayer()
    {
        tag = "Player";
    }
    void Reset()
    {
        socket = transform.Find("socket");
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
            Cmd_Shoot();
        }
    }

    private void GetMovement()
    {
        transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
        AudioSource.PlayClipAtPoint(audioDriving, transform.position, 1.0f);
    }
    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.localRotation = Quaternion.Euler(180, 0, 0);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            GetMovement();
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            GetMovement();
            return;
        }
    }
    [Command] public void Cmd_Shoot()
    {
        /*shootTimer += Time.deltaTime;
        if (shootTimer < shootCooldown)
        {
            return;
        }
*/
        //if (Input.GetButtonDown("Fire1"))
        //{
            GameObject obj = Instantiate(bulletPrefab, socket.position, socket.rotation) as GameObject;
            //Destroy(obj, 2.0f);
            //obj.GetComponent<Transform>().Translate(0, -bulletSpeed * Time.deltaTime, 0);
            NetworkServer.Spawn(obj);
            //shootTimer = 0;
        //}
    }


}
