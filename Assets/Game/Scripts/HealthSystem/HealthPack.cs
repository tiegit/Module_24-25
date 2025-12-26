using System.Collections;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 50;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private GameObject _effectPrefab;

    private void Start()
    {
        StartCoroutine(SelfDestroyCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealable healable))
        {
            healable.Heal(_healingAmount);

            if (_effectPrefab != null)
                Instantiate(_effectPrefab, transform.position, Quaternion.identity, null);

            Destroy(gameObject);
        }
    }

    private IEnumerator SelfDestroyCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Collider collider = GetComponent<Collider>();

        if (collider is SphereCollider sphereCollider)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
        }
    }
}
