using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [Header("Weapons & Shield")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Transform[] _laserPositionsArray;
    [SerializeField] private float _fireRate;
    private float _nextFire;
    [Space]
    [SerializeField] private GameObject _shieldObject;
    [Space]
    [SerializeField] private GameObject[] _damageEnginesObject;

    [Space(2)]
    [Header("Weapons Mod")]
    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _isSpeedBoostActive;
    [SerializeField] private bool _isShieldsActive;

    [Space(2)]
    [Header("Player Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _life = 3;
    public float Life 
    { 
        get {return _life;} 
    }

    [Space(2)]
    [Header("SFX & Explosion")]
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private GameObject _explosionPrefab;

    private void SetPowerup(Powerup index, bool option)
    {
        switch (index)
        {
            case Powerup.TripleShot:
                _isTripleShotActive = option;
                break;
            case Powerup.SpeedBoost:
                _isSpeedBoostActive = option;
                break;
            case Powerup.Shields:
                _isShieldsActive = option;
                break;
            default:
                Debug.LogError("This powerup doesn't exist");
                break;
        }
    }

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();       
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The AudioSource in " + gameObject.name +  " is not found");
        }

        transform.position = new Vector3(0, -2 ,0);
        _nextFire = -1f;
        
        _isTripleShotActive = false;
        _isSpeedBoostActive = false;
        _isShieldsActive = false;

        if(_damageEnginesObject == null)
        {
            Debug.Log("The Engine Objects are missing");
        }
        else
        {
            foreach (var damageEngine in _damageEnginesObject)
            {
                damageEngine.SetActive(false);
            }
        }
    }

    private void FireLaser()
    {
        if(_isTripleShotActive)
        {
            foreach(Transform pos in _laserPositionsArray)
            {
                GameObject laser = Instantiate(_laserPrefab, pos.position, Quaternion.identity);
                laser.GetComponent<LaserBehaviour>().ParentTag = "Player";
                laser.transform.SetParent(_gameManager.laserContainer.transform);
            }
        }
        else 
        {
            GameObject laser = Instantiate(_laserPrefab, _laserPositionsArray[0].position, Quaternion.identity);
            laser.GetComponent<LaserBehaviour>().ParentTag = "Player";
            laser.transform.SetParent(_gameManager.laserContainer.transform);
        }

        _audioSource.PlayOneShot(_laserSoundClip);
        _nextFire = Time.time + _fireRate;
    }

    private float CalculateSpeed()
    {
        if(_isSpeedBoostActive)
        {
            return _speedMultiplier * _speed;
        }

        return _speed;
    }

    private void CalculatePos()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, .0f);
        float currSpeed = CalculateSpeed();
        transform.Translate(direction * currSpeed * Time.deltaTime);

        transform.position = new Vector3(
                            Mathf.Clamp(transform.position.x, -_gameManager.screenX, _gameManager.screenX),
                            Mathf.Clamp(transform.position.y, -_gameManager.screenY + _gameManager.offsetY, _gameManager.screenY - _gameManager.offsetY),
                            .0f);
    }

    private void Update()
    {
        CalculatePos();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();            
        }
    }

    public void TripleShotActive(float time)
    {
        StartCoroutine(PowerupTurnOn(time, Powerup.TripleShot));
    }

    public void SpeedBoostActive(float time)
    {
        StartCoroutine(PowerupTurnOn(time, Powerup.SpeedBoost));
    }

    public void ShieldsActive(bool option)
    {
        _shieldObject.SetActive(option);
        SetPowerup(Powerup.Shields, option);
    }

    IEnumerator PowerupTurnOn(float time, Powerup index)
    {
        SetPowerup(index, true);
        yield return new WaitForSeconds(time);
        SetPowerup(index, false);
    }

    public void TakeDamage(float dmg)
    {
        if(_isShieldsActive)
        {
            ShieldsActive(false);
            return;
        }
        
        _life -= dmg;
        _gameManager.GetComponent<UIManager>().UpdateLivesImage();

        VisualizationDamage();
    }

    private void VisualizationDamage()
    {
        if(_life < 3)
        {
            _damageEnginesObject[0].SetActive(true);
        }
        if(_life < 2)
        {
            _damageEnginesObject[1].SetActive(true);
        }
        if(_life <= 0)
        {
            _gameManager.PlayerDeath();
            PlayerExplosion();
        }
    }

    private void PlayerExplosion()
    {
        gameObject.SetActive(false);
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.SetParent(_gameManager.explosionContainer.transform);
        Destroy(this.gameObject);
    }
}
