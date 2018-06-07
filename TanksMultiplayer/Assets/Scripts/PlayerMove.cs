using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Photon.MonoBehaviour
{
    public PhotonView photonView;
    [Header("General booleans")]
    public bool devTesting = false;
    [Space]
    [Header("General floats")]
    public float moveSpeed = 1.5f;
    [Space]
    public GameObject playerCam;
    public Text playerName;
    private Vector3 selfPos;
    private GameObject sceneCam;
    public Color enemyTextColor;
    public GameObject bulletPrefab;
    public SpriteRenderer sprite;

    void Awake()
    {
        if (!devTesting && photonView.isMine)
        {
            playerName.text = PhotonNetwork.playerName;
        }

        if (!devTesting && !photonView.isMine)
        {
            playerName.text = photonView.owner.name;
            playerName.color = enemyTextColor;
        }
    }
    private void Update()
    {
        if (!devTesting)
        {
            if (photonView.isMine)
            {
                checkInput();
            }
            else
            {
                smoothNetMovement();
            }
        }
        else
        {
            checkInput();
        }

    }
    private void checkInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += move * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            sprite.flipX = false;
            photonView.RPC("onSpriteFlipFalse", PhotonTargets.Others);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            sprite.flipX = true;
            photonView.RPC("onSpriteFlipTrue", PhotonTargets.Others);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            sprite.flipX = true;
            photonView.RPC("onSpriteFlipTrue", PhotonTargets.Others);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            sprite.flipX = false;
            photonView.RPC("onSpriteFlipFalse", PhotonTargets.Others);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }

    }
    [PunRPC]
    private void onSpriteFlipTrue()
    {
        sprite.flipX = true;
    }
    [PunRPC]
    private void onSpriteFlipFalse()
    {
        sprite.flipX = false;
    }

    private void shoot()
    {
        if (!devTesting)
        {
            if (sprite.flipX == false)
            {
                GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                obj.GetComponent<PhotonView>().RPC("changeDirection_Left", PhotonTargets.AllBuffered);
            }
            else
            {
                GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
                obj.GetComponent<PhotonView>().RPC("changeDirection_Left", PhotonTargets.AllBuffered);
            }
        }
    }
    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 10);
    }
    private void OnPhotonSerializedView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
