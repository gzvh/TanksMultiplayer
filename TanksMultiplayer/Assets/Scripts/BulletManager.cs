using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Photon.MonoBehaviour
{

    public bool movingDirection = false;
    public float movingSpeed = 4f;
    public float destroyTime = 2f;


    [PunRPC]
    public void changeDirection_Left()
    {
        movingDirection = true;
    }
	[PunRPC]
	private void destroyOBJ()
	{
		Destroy(gameObject);
	}

    void Update()
    {
        if (!movingDirection)
            transform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
        else
        {
            transform.Translate(Vector2.left * movingSpeed * Time.deltaTime);
        }
    }
	void OnTriggerEnter2D(Collider2D other)
	{
		if(!photonView.isMine)
		{
			return;
		}
		PhotonView target = other.gameObject.GetComponent<PhotonView>();
		if(target != null && (!target.isMine || target.isSceneView))
		{
			if(other.tag == "Player")
			{
				other.GetComponent<PhotonView>().RPC("reduceHealth", PhotonTargets.All);
				this.GetComponent<PhotonView>().RPC("destroyOBJ", PhotonTargets.All);
			}
		}
	}
}
