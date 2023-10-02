using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTowertoEnemy : MonoBehaviour
{
    public float detectionRadius = 5f; // 탐지 범위
    public LayerMask enemyLayer; // 탐지할 적의 레이어
    public float rotateSpeed = 2f; // 회전 속도 추가
    private GameObject detectedEnemy; // 탐지한 적 오브젝트
    private Quaternion initialRotation; // 초기 회전값

    private void Start()
    {
        // 초기 회전값 저장
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // 탐지 범위 내에 있는 적을 찾습니다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        if (colliders.Length > 0)
        {
            // 가장 가까운 적을 찾습니다.
            float closestDistance = Mathf.Infinity;
            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    detectedEnemy = collider.gameObject;
                }
            }

            // 탐지한 적을 바라보게 회전시킵니다.
            Vector3 lookAtPosition = new Vector3(detectedEnemy.transform.position.x, transform.position.y, detectedEnemy.transform.position.z);
            Vector3 direction = lookAtPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
        else
        {
            // 탐지 범위 내에 적이 없으면 초기 회전값으로 회전합니다.
            transform.rotation = initialRotation;
            detectedEnemy = null;
        }
    }
}