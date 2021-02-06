using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SpawnManager is null!");
            }
            return _instance;
        }
    }

    [SerializeField] Transform _enemyContainer;
    [SerializeField] GameObject _enemyPref;
    [SerializeField] GameObject[] _powerupPrefs;
    [SerializeField] GameObject _ammoPowerupPref, _healthPowerupPref;

    void Awake()
    {
        _instance = this;
    }

    public void StartSpawning()
    {
        StartCoroutine("EnemySpawnRtn");
        StartCoroutine("IncreaseEnemySpawnRateRtn");
        StartCoroutine("PowerupSpawnRtn");
        StartCoroutine("AmmoSpawnRtn");
        StartCoroutine("HealthSpawnRtn");
    }

    IEnumerator EnemySpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(1f, 2f) - spawnRateModifier;

            yield return new WaitForSeconds(waitTime);
            float randomX = Random.Range(-9.5f, 9.5f);
            Instantiate(_enemyPref, new Vector2(randomX, 7.77f), Quaternion.identity, _enemyContainer);
        }
    }
    float spawnRateModifier, timeBeforeSpawnRateModifierChange = 20f;
    IEnumerator IncreaseEnemySpawnRateRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            yield return new WaitForSeconds(timeBeforeSpawnRateModifierChange);
            if (spawnRateModifier < 0.9f)
                spawnRateModifier += 0.1f;
            else
            {
                //print("breaking out of enemy spawn rate rtn");
                yield break;
            }
                
            timeBeforeSpawnRateModifierChange = timeBeforeSpawnRateModifierChange + (timeBeforeSpawnRateModifierChange / 10);
            //print("Current spawn rate modifier: " + spawnRateModifier);
            //print($"spawn rate changing in {timeBeforeSpawnRateModifierChange} seconds");
        }
    }

    IEnumerator PowerupSpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(7f, 12f);

            int index = Random.Range(0, _powerupPrefs.Length);
            Vector2 spawnPos = new Vector2(Random.Range(-9.5f, 9.5f), 7.77f);

            yield return new WaitForSeconds(waitTime);
            Instantiate(_powerupPrefs[index], spawnPos, Quaternion.identity);
        }
    }

    IEnumerator AmmoSpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(12f, 20f);

            Vector2 spawnPos = new Vector2(Random.Range(-9.5f, 9.5f), 7.77f);

            yield return new WaitForSeconds(waitTime);
            Instantiate(_ammoPowerupPref, spawnPos, Quaternion.identity);
        }
    }

    IEnumerator HealthSpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(20f, 30f);

            Vector2 spawnPos = new Vector2(Random.Range(-9.5f, 9.5f), 7.77f);

            yield return new WaitForSeconds(waitTime);
            Instantiate(_healthPowerupPref, spawnPos, Quaternion.identity);
        }
    }


}
