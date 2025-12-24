using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 50;
    [SerializeField] private GameObject _effectPrefab;

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
}
