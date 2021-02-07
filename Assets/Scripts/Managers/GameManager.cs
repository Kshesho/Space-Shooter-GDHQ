using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is null!");
            }
            return _instance;
        }
    }

    public bool PlayerAlive { get; private set; } = true;
    public void PlayerDied()
    {
        PlayerAlive = false;
        UIManager.Instance.EnableGameOverContainer();
    }

    public int MultiplierLevel { get; private set; } = 1;
    public int Score { get; private set; }
    float _scoreMultiplier = 1;
    public void UpdateScore(int scoreToAdd)
    {
        float addedScore = scoreToAdd * _scoreMultiplier;
        Score += Mathf.RoundToInt(addedScore);
        UIManager.Instance.UpdateScoreText(Score);
    }
    public void RaiseScoreMultiplier()
    {
        MultiplierLevel += 1;

        switch (MultiplierLevel)
        {
            case 2:
                _scoreMultiplier = 1.1f;
                break;
            case 3:
                _scoreMultiplier = 1.2f;
                break;
            case 4:
                _scoreMultiplier = 1.4f;
                break;
            case 5:
                _scoreMultiplier = 1.6f;
                break;
            case 6:
                _scoreMultiplier = 1.8f;
                break;
            case 7:
                _scoreMultiplier = 2f;
                break;
            case 8:
                _scoreMultiplier = 2.5f;
                break;
            case 9:
                _scoreMultiplier = 4f;
                break;
            case 10:
                _scoreMultiplier = 5f;
                break;
        }
        UIManager.Instance.UpdateMultiplierText(MultiplierLevel);

        print("Score multiplier: " + _scoreMultiplier);
    }

    //the time in seconds that it takes the player to overheat if they activate thrusters from 0
    public float ThrusterOverheatTimer { get; } = 1.5f;

    public bool GamePaused { get; private set; }

    [SerializeField] Animator _mainCameraAnim;
    //------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (!PlayerAlive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitToMainMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------------

    void PauseGame()
    {
        Time.timeScale = 0;
        UIManager.Instance.TogglePauseMenu(true);
        GamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        UIManager.Instance.TogglePauseMenu(false);
        GamePaused = false;
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ShakeTheCamera()
    {
        _mainCameraAnim.SetTrigger("shake");
    }


}
