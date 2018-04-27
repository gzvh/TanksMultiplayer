using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : Photon.MonoBehaviour
{

    #region PUBLIC PROPERTIES
    public float movementSpeed = 1.5f;
    public AudioClip audioDriving = null;
    #endregion

    #region MONOBEHAVIOUR MESSAGES
    void Start()
    {

    }
    void Update()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
        Move();
    }
    #endregion
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
}
