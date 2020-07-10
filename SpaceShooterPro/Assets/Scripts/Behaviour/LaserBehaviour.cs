using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Stats")]
    [SerializeField] private float _speed;
    public void ReverseSpeed()
    {
        _speed *= -1;
    }
    [SerializeField] private float _damage;
    public float Damage 
    { 
        set {_damage = value;} 
    }
    [SerializeField] private float _lifeTime;
    private string _parentTag;
    public string ParentTag 
    { 
        set {_parentTag = value;} 
    }

    private void Start()
    {
         _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();   
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }

        Destroy(this.gameObject, _lifeTime);
    }

    private void Update() 
    {
        transform.position += Vector3.up * _speed *Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Enemy") && _parentTag == "Player")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy != null)
            {
                StartCoroutine(enemy.DestroyEnemy());
                _gameManager.CurrScore += enemy.Score;
                _gameManager.GetComponent<UIManager>().SetScoreText();
            }
            Destroy(this.gameObject);           
        }

        if(other.gameObject.CompareTag("Player") && _parentTag == "Enemy")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(_damage);
            }

            Destroy(this.gameObject);
        }
    }
}
