using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("The Animator in " + gameObject.name + " is missing.");
            Destroy(gameObject);
        }
        else 
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        float animTime = _animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Destroy(gameObject, animTime);
    }
}
