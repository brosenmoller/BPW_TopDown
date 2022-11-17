using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * (target.position - transform.position).normalized;
    }
}
