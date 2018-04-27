#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && ! UNITY_5_3) || UNITY_2017
#define UNITY_MIN_5_4
#endif

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Player manager. 
/// Handles fire Input and Beams.
/// </summary>
public class PlayerManager : Photon.PunBehaviour, IPunObservable
{

    #region Public Variables
    [Tooltip("The Beams GameObject to control")]
    public GameObject Beams;
    [Tooltip("The current Health of our player")]
    public float Health = 1f;
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    [Tooltip("The Player's UI GameObject Prefab")]
    public GameObject PlayerUiPrefab;
    #endregion

    #region Private Variables
    //True, when the user is firing
    bool IsFiring;
    #endregion

    #region MonoBehaviour CallBacks
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    /// <summary>
    /// MonoBehaviour method called when the Collider 'other' enters the trigger.
    /// Affect Health of the Player if the collider is a beam
    /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
    /// One could move the collider further away to prevent this or check if the beam belongs to the player.
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.tag.Contains("Enemy"))
        {
            return;
        }
        this.Health -= 0.1f;
    }
    void Awake()
    {
        if (this.Beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            this.Beams.SetActive(false);
        }
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
#if UNITY_MIN_5_4
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
    }
    public void OnDisable()
    {
#if UNITY_5_4_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
    }
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {
        if (photonView.isMine)
        {
            ProcessInputs();
            if (this.Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
        // trigger Beams active state 
        if (Beams != null && this.IsFiring != this.Beams.GetActive())
        {
            this.Beams.SetActive(this.IsFiring);
        }
    }
#if !UNITY_MIN_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
#endif
    void CalledOnLevelWasLoaded(int level)
    {
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
    #endregion

    #region Custom

#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif
    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            // we don't want to fire when we interact with UI buttons for example. IsPointerOverGameObject really means IsPointerOver*UI*GameObject
            // notice we don't use on on GetbuttonUp() few lines down, because one can mouse down, move over a UI element and release, which would lead to not lower the isFiring Flag.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (!this.IsFiring)
            {
                this.IsFiring = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (this.IsFiring)
            {
                this.IsFiring = false;
            }
        }
    }
    #endregion

    #region IPunObservable implementation
    /*void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
        }
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    } */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.IsFiring);
            stream.SendNext(this.Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }
    #endregion
}