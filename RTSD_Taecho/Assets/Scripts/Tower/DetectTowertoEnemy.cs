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
    private List<GameObject> detectedEnemies = new List<GameObject>();

    private void Start()
    {
        // 초기 회전값 저장
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        RotateToDetectedEnemy();
    }

    private void RotateToDetectedEnemy()
    {
        // 탐지 범위 내에 있는 적을 찾습니다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        // 가장 가까운 적을 찾기 위한 변수 초기화
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            // 탐지된 오브젝트를 리스트에 추가하고, 필요한 로직을 수행합니다.
            if (!detectedEnemies.Contains(collider.gameObject))
            {
                detectedEnemies.Add(collider.gameObject);
            }

            // 적과의 거리 계산
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                // 더 가까운 적을 찾으면 업데이트
                closestDistance = distance;
                closestEnemy = collider.gameObject;
            }
        }

        // 가장 가까운 적이 있을 경우
        if (closestEnemy != null)
        {
            // 탐지한 적을 바라보는 로직을 여기에 추가합니다.
            Vector3 lookAtPosition = new Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z);
            Vector3 direction = lookAtPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 타워가 정확히 적을 바라보게 하려면 angle을 90도 더해줍니다.
            angle += 90f;

            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
        else
        {
            // 탐지 범위 내에 적이 없으면 초기 회전값으로 회전합니다.
            transform.rotation = initialRotation;
        }
    }

}
