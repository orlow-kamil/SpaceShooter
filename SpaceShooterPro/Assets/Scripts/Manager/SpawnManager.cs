using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager _gameManager;
    private IEnumerator _coroutine;

    [Header("Enemy & Powerup")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<GameObject> _powerupPrefabsList;

    [Space(2)]
    [Header("Time")]
    [SerializeField] private float _startTime;
    [SerializeField] private float _spawnEnemyTime;
    [SerializeField] private float _spawnPowerupTime;

    private void Start()
    {
        _gameManager = gameObject.GetComponent<GameManager>();       
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }
    }

    public void StartSpawnsObject()
    {
        _coroutine = SpawnEnemy(_startTime, _spawnEnemyTime);
        StartCoroutine(_coroutine);

        _coroutine = SpawnPowerup(_startTime, _spawnPowerupTime);
        StartCoroutine(_coroutine);
    }

    private void SetupNewEnemy()
    {
        float randomX = Random.Range(-_gameManager.screenX, _gameManager.screenX);
        Vector3 pos = new Vector3(randomX, _gameManager.screenY, 0);

        GameObject newEnemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        newEnemy.transform.SetParent(_gameManager.enemyContainer.transform);

        _gameManager.EnemiesList.Add(newEnemy);
    }

    private void SetupNewPowerup()
    {
        float randomX = Random.Range(-_gameManager.screenX, _gameManager.screenX);
        int randomIndex = Random.Range(0, _powerupPrefabsList.Count);

        Vector3 pos = new Vector3(randomX, _gameManager.screenY, 0);
        GameObject newPowerup = Instantiate(_powerupPrefabsList[randomIndex], pos, Quaternion.identity);
        newPowerup.transform.SetParent(_gameManager.powerupContainer.transform);
    }

    private IEnumerator SpawnEnemy(float startTime, float waitTime)
    {
        yield return new WaitForSeconds(startTime);
        while(!_gameManager.IsPlayerDied)
        {
            SetupNewEnemy();
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForSeconds(startTime);
        _gameManager.DestroyEnemies();
    }

    private IEnumerator SpawnPowerup(float startTime, float waitTime)
    {
        yield return new WaitForSeconds(startTime);
        while(!_gameManager.IsPlayerDied)
        {
            SetupNewPowerup();
            float randomTime = Random.Range(waitTime, 2 * waitTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
