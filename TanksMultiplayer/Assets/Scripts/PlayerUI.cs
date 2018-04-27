using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerUI : MonoBehaviour
{
    #region Public Properties
    [Tooltip("UI Text to display Player's Name")]
    public Text PlayerNameText;
    [Tooltip("UI Slider to display Player's Health")]
    public Slider PlayerHealthSlider;
    [Tooltip("Pixel offset from the player target")]
    public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);
    #endregion


    #region Private Properties
    PlayerManager _target;
    float _characterControllerHeight = 0f;
    Transform _targetTransform;
    Vector3 _targetPosition;
    #endregion


    #region MonoBehaviour Messages
    void Awake()
    {
        this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
    }
    void Update()
    {
        // Reflect the Player Health
        if (PlayerHealthSlider != null)
        {
            PlayerHealthSlider.value = _target.Health;
        }
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    #endregion

    #region Public Methods
    public void SetTarget(PlayerManager target)
    {
        if (target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        _target = target;
        if (PlayerNameText != null)
        {
            PlayerNameText.text = _target.photonView.owner.NickName;
        }
    }
    #endregion
}