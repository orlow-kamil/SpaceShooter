using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _animator;
    private AudioSource _audioSource;

    [Header("Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private int _score;
    public int Score 
    { 
        get { return _score;} 
    }

    [Space(2)]
    [Header("Simply AI")]
    [SerializeField][Range(0,5)] private float _dodge;
    [SerializeField] private Vector2 _maneuverTime;
    private float _currDodgeX;
    private bool _isAlive;

    [Space(2)]
    [Header("Shooting")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private List<Transform> _gunsPositionList;
    [SerializeField] private Vector2 _fireRate;

    private void Start() 
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();   
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
        }

        _animator = gameObject.GetComponentInChildren<Animator>();
        if(_animator == null)
        {
            Debug.LogError("The Animator in " + gameObject.name +  " is not found");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The AudioSource in " + gameObject.name +  " is not found");
        }

        _isAlive = true;
        StartCoroutine(Evade());
        StartCoroutine(Shoot());
    }

    private void Move()
    {
        transform.position += new Vector3(_currDodgeX, -_speed, .0f) * Time.deltaTime;

        transform.position = new Vector3(
                            Mathf.Clamp(transform.position.x, -_gameManager.screenX, _gameManager.screenX),
                            transform.position.y,
                            .0f);

        if(transform.position.y < -_gameManager.screenY)
        {
            float randomX = Random.Range(-_gameManager.screenX, _gameManager.screenX);
            transform.position = new Vector3(randomX, _gameManager.screenY);
        }
    }

    private void Update()
    {
        if(_isAlive)
        {
            Move();
        }
    }

    private IEnumerator Evade()
    {
        while (_isAlive) 
		{
			_currDodgeX = Random.Range (-_dodge, _dodge);
			yield return new WaitForSeconds (Random.Range(_maneuverTime.x, _maneuverTime.y));			
            _currDodgeX = .0f;
			yield return new WaitForSeconds (Random.Range(_maneuverTime.x, _maneuverTime.y));
		}
        yield break;
    }

    private IEnumerator Shoot()
    {
        while(_isAlive)
        {
            yield return new WaitForSeconds(Random.Range(_fireRate.x, _fireRate.y));
            foreach(Transform pos in _gunsPositionList)
            {
                GameObject laser = Instantiate(_laserPrefab, pos.position, Quaternion.identity);
                laser.GetComponent<LaserBehaviour>().ParentTag = "Enemy";
                laser.GetComponent<LaserBehaviour>().ReverseSpeed();
                laser.GetComponent<LaserBehaviour>().Damage = 0.5f;
                laser.transform.SetParent(_gameManager.laserContainer.transform);
            }
        }
        yield break;
    }

    public IEnumerator DestroyEnemy()
    {
        _isAlive = false;
        GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();

        _animator.SetTrigger("onEnemyDeath");
        float animTime = _animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        _audioSource.Play();
        yield return new WaitForSeconds(animTime);

        Destroy(this.gameObject);
    }
}
