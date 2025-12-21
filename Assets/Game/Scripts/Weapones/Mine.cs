using System.Collections;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class Mine : MonoBehaviour
{
    [SerializeField] private float _activationRadius = 1f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private float _explosionDelay = 2f;
    [SerializeField] private float _blinkSpeed = 5f;
    [SerializeField] private Renderer _mineRenderer;
    [SerializeField] private GameObject _explosionEffect;

    private SphereCollider _collider;

    private Color _originalColor;

    private Coroutine _mineActivationCourutine;
    private Coroutine _blinkCoroutine;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _activationRadius;

        if (_mineRenderer != null)        
            _originalColor = _mineRenderer.material.color;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_mineActivationCourutine == null && other.TryGetComponent(out IDamagable damagable))
            _mineActivationCourutine = StartCoroutine(ActivateMine());
    }

    private IEnumerator ActivateMine()
    {
        _blinkCoroutine = StartCoroutine(BlinkCoroutine());

        yield return _blinkCoroutine;

        _blinkCoroutine = null;

        Explode();
    }
    
    private IEnumerator BlinkCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _explosionDelay)
        {
            if (_mineRenderer != null)
            {
                float blinkValue = Mathf.PingPong(elapsedTime * _blinkSpeed, 1f);
                Color blinkColor = Color.Lerp(_originalColor, Color.white, blinkValue);
                _mineRenderer.material.color = blinkColor;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (_mineRenderer != null)
            _mineRenderer.material.color = _originalColor;
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_damage);
        }

        if (_explosionEffect != null)
            Instantiate(_explosionEffect, transform.position, _explosionEffect.transform.rotation, null);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _activationRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
