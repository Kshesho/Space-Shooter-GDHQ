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
    [SerializeField] GameObject _ammoPowerupPref;

    void Awake()
    {
        _instance = this;
    }

    public void StartSpawning()
    {
        StartCoroutine("EnemySpawnRtn");
        StartCoroutine("PowerupSpawnRtn");
        StartCoroutine("AmmoSpawnRtn");
    }

    IEnumerator EnemySpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(1f, 2f);

            yield return new WaitForSeconds(waitTime);
            float randomX = Random.Range(-9.5f, 9.5f);
            Instantiate(_enemyPref, new Vector2(randomX, 7.77f), Quaternion.identity, _enemyContainer);
        }
    }

    IEnumerator PowerupSpawnRtn()
    {
        while (GameManager.Instance.PlayerAlive)
        {
            float waitTime = Random.Range(4f, 9f);

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
            float waitTime = 8f;

            Vector2 spawnPos = new Vector2(Random.Range(-9.5f, 9.5f), 7.77f);

            yield return new WaitForSeconds(waitTime);
            Instantiate(_ammoPowerupPref, spawnPos, Quaternion.identity);
        }
    }


}
