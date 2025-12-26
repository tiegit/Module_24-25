using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Mine : MonoBehaviour
{
    private const string IsActivated = "_IsActivated";

    [SerializeField] private float _activationRadius = 1f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private float _explosionDelay = 2f;

    [SerializeField] private GameObject _explosionEffect;

    private SphereCollider _collider;

    private Coroutine _mineActivationCourutine;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _activationRadius;

        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_mineActivationCourutine == null && other.TryGetComponent(out IDamagable damagable))
            _mineActivationCourutine = StartCoroutine(ActivateMine());
    }

    private IEnumerator ActivateMine()
    {
        _renderer.material.SetInt(IsActivated, 1);

        float elapsedTime = 0f;

        while (elapsedTime < _explosionDelay)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Explode();
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
