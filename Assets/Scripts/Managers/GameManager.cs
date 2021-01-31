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

    public int Score { get; private set; }
    public void UpdateScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        UIManager.Instance.UpdateScoreText(Score);
    }

    //the time in seconds that it takes the player to overheat if they activate thrusters from 0
    public float ThrusterOverheatTimer { get; } = 5;

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
                SceneManager.LoadScene(1);
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------------

    #region Powerups

    public float SpeedBoostActiveTime
    {
        get
        {
            return _speedBoostActiveTime;
        }
    }
    [SerializeField] float _speedBoostActiveTime = 3f;

    public bool SpeedBoostActive { get; private set; }
    public void ActivateSpeedBoost()
    {
        SpeedBoostActive = true;
        StopCoroutine("DeactivateSpeedBoost");
        StartCoroutine("DeactivateSpeedBoost");
    }
    IEnumerator DeactivateSpeedBoost()
    {
        yield return new WaitForSeconds(_speedBoostActiveTime);
        SpeedBoostActive = false;
    }

    #endregion

    public void ShakeTheCamera()
    {
        _mainCameraAnim.SetTrigger("shake");
    }


}
