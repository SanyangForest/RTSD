using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    public float MoveSpeed = 0.0f;
    [SerializeField]
    private Vector3 MoveDirection = Vector3.zero;

    // public float moveSpeed => MoveSpeed;

    private void Update()
    {
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        MoveDirection = direction;
    }
}