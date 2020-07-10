using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController _currPlayer;
    public PlayerController CurrPlayer 
    { 
        get {return _currPlayer;} 
    }

    private List<GameObject> _enemiesList;
    public List<GameObject> EnemiesList 
    { 
        get {return _enemiesList;} 
        set {_enemiesList = value;}
    }

    [Header("Asteroid")]
    [SerializeField] private List<GameObject> _asteroidsList;
    [SerializeField] private int _asteroidsAmount;
    public int AsteroidsAmount 
    { 
        get { return _asteroidsAmount;}
        set {_asteroidsAmount = value;}
    }

    [Space(2)]
    [Header("Gameplay objects container")]
    public GameObject laserContainer;
    public GameObject explosionContainer;
    public GameObject enemyContainer;
    public GameObject powerupContainer;

    private bool _isPlayerDied;
    public bool IsPlayerDied 
    { 
        get {return _isPlayerDied;} 
        set {_isPlayerDied = value;} 
    }

    [Space(2)]
    [Header("Screen dim")]
    public float screenX;
    public float screenY;
    public float offsetY;

    [Space(2)]
    [Header("Game Stats")]
    [SerializeField] private int _currScore;
    public int CurrScore { get; set; }

    private KeyCode _restartKeyCode = KeyCode.R;
    private KeyCode _quitKeyCode = KeyCode.Escape;

    private void Start() 
    {
        _currPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if(_currPlayer == null)
        {
            Debug.LogError("Current player didn't find.");
        }

        _isPlayerDied = false;
        _currScore = 0;
        _enemiesList = new List<GameObject>();
        _asteroidsAmount = _asteroidsList.Count;
    }

    public void DestroyEnemies()
    {
        foreach(GameObject enemy in _enemiesList)
        {
            Destroy(enemy);
        }
        _enemiesList = null;
    }

    public void PlayerDeath()
    {
        IsPlayerDied = true;
        gameObject.GetComponent<UIManager>().SetActiveGameOverText(true);

    }

    private void Update() 
    {
        if(Input.GetKeyDown(_restartKeyCode) && _isPlayerDied)
        {
            SceneManager.LoadScene(1);
        }

        if(Input.GetKeyDown(_quitKeyCode) && _isPlayerDied)
        {
            Debug.Log("The Game is quitting!");
            Application.Quit();
        }
    }
}
