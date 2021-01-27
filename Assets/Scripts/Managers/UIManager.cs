using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager is null!");
            }
            return _instance;
        }
    }

    [SerializeField] Text _scoreText;

    [SerializeField] GameObject _singleShotElements, _tripleShotElements;
    [SerializeField] Text _singleShotAmmoCountText, _tripleShotAmmoCountText;
    [SerializeField] AmmoNotifications _ammoNotifications;
    [SerializeField] Image _ammoImage;
    [SerializeField] Sprite _singleShotSprite, _tripleShotSprite;

    [SerializeField] Image _livesDisplay;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] GameObject _gameOverContainer;

    //------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        _instance = this;    
    }
    //------------------------------------------------------------------------------------------------------------------

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLivesImage(int lives)
    {
        _livesDisplay.sprite = _livesSprites[lives];
    }

    public void UpdateAmmoDisplay_TripleShot(int ammoCount)
    {
        _tripleShotAmmoCountText.text = ammoCount.ToString();
    }
    public void UpdateAmmoDisplay_SingleShot(int ammoCount)
    {
        _singleShotAmmoCountText.text = ammoCount.ToString();
    }
    public void OutOfAmmoFlash()
    {
        _ammoNotifications.OutOfAmmoIndicator();
    }
    public void TripleShotUI()
    {
        _singleShotElements.SetActive(false);
        _tripleShotElements.SetActive(true);
        _ammoImage.sprite = _tripleShotSprite;
    }
    public void SingleShotUI()
    {
        _tripleShotElements.SetActive(false);
        _singleShotElements.SetActive(true);
        _ammoImage.sprite = _singleShotSprite;
    }

    public void EnableGameOverContainer()
    {
        _gameOverContainer.SetActive(true);
    }


}
