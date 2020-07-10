using UnityEngine;


public enum Powerup
{
    TripleShot,
    SpeedBoost,
    Shields
}

public class PowerupBehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _powerupTime;

    [Space(2)]
    [Header("Category")]
    [SerializeField] private Powerup _currentPowerup;

    [Space(2)]
    [Header("SFX")]
    [SerializeField] private AudioClip _powerupAudioClip;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();       
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }
    }

    private void Move()
    {
        transform.position += Vector3.down * _speed * Time.deltaTime;   

        if(transform.position.y <= - _gameManager.screenY)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Move();
    }

    private void ChoosePowerup(PlayerController player)
    {
        switch (_currentPowerup)
        {
            case Powerup.TripleShot:
                player.TripleShotActive(_powerupTime);
                break;
            case Powerup.SpeedBoost:
                player.SpeedBoostActive(_powerupTime);
                break;
            case Powerup.Shields:
                player.ShieldsActive(true);
                break;
            default:
                Debug.LogError("This powerup doesn't exist");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.transform.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if(player != null)
            {
                AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);
                ChoosePowerup(player);
            }

            Destroy(this.gameObject);
        }
    }
}
