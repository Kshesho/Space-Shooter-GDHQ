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

    [SerializeField] Text _scoreText, _multiplierText;

    [SerializeField] GameObject _singleShotElements, _tripleShotElements;
    [SerializeField] Text _singleShotAmmoCountText, _tripleShotAmmoCountText;
    [SerializeField] AmmoNotifications _ammoNotifications;
    [SerializeField] Image _ammoImage;
    [SerializeField] Sprite _singleShotSprite, _tripleShotSprite;

    [SerializeField] Image _livesDisplay;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] GameObject _gameOverContainer;

    [SerializeField] Image _thrusterBar;
    [SerializeField] Color _barBaseColor, _barOverheatColor;
    [SerializeField] GameObject _overheatThrusterBar, _speedBoostThrusterBar;
    [SerializeField] GameObject _pauseMenu;

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
    
    public void UpdateThrusterUI(bool boosting, float overheatPercent)
    {
        float velocity = 0;
        float baseFillAmount = 0.1f;
        float maxFillAmount = 1f;
        //I want to make sure that the amount of seconds the bar takes to fill up is 
        //the same amount as the overheat timer. But I'm not sure how to calculate that
        float lerpSpeed = Time.deltaTime / 5;

        float currentOverheatPercentage = (maxFillAmount - baseFillAmount) * overheatPercent;

        if (boosting)
        {
            _thrusterBar.fillAmount = baseFillAmount + currentOverheatPercentage;
            //_thrusterBar.fillAmount = Mathf.SmoothDamp(_thrusterBar.fillAmount, maxFillAmount, ref velocity, lerpSpeed);
        }
        else
        {
            _thrusterBar.fillAmount = baseFillAmount + currentOverheatPercentage;
            //_thrusterBar.fillAmount = Mathf.SmoothDamp(_thrusterBar.fillAmount, baseFillAmount, ref velocity, lerpSpeed);
        }
    }
    public void ThrusterOverheatedUI()
    {
        _speedBoostThrusterBar.SetActive(false);
        _thrusterBar.gameObject.SetActive(false);
        _thrusterBar.fillAmount = 0.1f;
        _overheatThrusterBar.SetActive(true);
        //trigger animation that turns the thruster bar red and white repeatedly
        //disable normal thruster bar
        //enable an overheat bar that animates back to 0,1 with the smoke 
    }
    public void ThrusterOverheatCompleteUI()
    {
        _overheatThrusterBar.SetActive(false);
        _thrusterBar.gameObject.SetActive(true);
    }
    
    public void ThrusterSpeedBoostUI()
    {
        _thrusterBar.gameObject.SetActive(false);
        _overheatThrusterBar.SetActive(false);
        _speedBoostThrusterBar.SetActive(true);
    }

    public void UpdateMultiplierText(int value)
    {
        _multiplierText.text = "x" + value;
        if (value == 10)
        {
            _multiplierText.GetComponent<Animator>().SetTrigger("rainbowTime");
        }
    }

    public void EnableGameOverContainer()
    {
        _gameOverContainer.SetActive(true);
    }

    public void TogglePauseMenu(bool on)
    {
        _pauseMenu.SetActive(on);
    }


}
