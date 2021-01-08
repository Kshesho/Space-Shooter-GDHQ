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
    [SerializeField] Image _livesDisplay;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] GameObject _gameOverContainer;

    //------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        _instance = this;    
    }

    void Update()
    {
        
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

    public void EnableGameOverContainer()
    {
        _gameOverContainer.SetActive(true);
    }


}
