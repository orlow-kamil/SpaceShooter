using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Stats")]
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int _damage;

    [Space(2)]
    [Header("Explosion")]
    [SerializeField] private GameObject _explosionPrefab;


    private void Start() 
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();        
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void SetExplosion()
    {
        GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.SetParent(_gameManager.explosionContainer.transform);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
            LaserBehaviour laser = other.gameObject.GetComponent<LaserBehaviour>();
            if(laser != null)
            {
                Destroy(laser.gameObject);
            }
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(_damage);
            }
        }

        _gameManager.AsteroidsAmount--;

        if(_gameManager.AsteroidsAmount == 0)
        {
            _gameManager.GetComponent<SpawnManager>().StartSpawnsObject();
        }

        SetExplosion();
    }
}
