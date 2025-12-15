using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _distanceToCheck;

    public bool IsTouches() => Physics.CheckSphere(transform.position, _distanceToCheck, _mask.value);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _distanceToCheck);
    }
}
