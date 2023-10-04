using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTowertoEnemy : MonoBehaviour
{
    [SerializeField]
    public float detectionRadius = 5f; // 탐지 범위
    public LayerMask enemyLayer; // 탐지할 적의 레이어
    public float rotateSpeed = 2f; // 회전 속도 추가
    private GameObject detectedEnemy; // 탐지한 적 오브젝트
    private Quaternion initialRotation; // 초기 회전값
    private List<GameObject> detectedEnemies = new List<GameObject>();

    // AttackRange 프로퍼티 제거

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
            GameObject enemy = collider.gameObject;

            // 적과의 거리 계산
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                // 더 가까운 적을 찾으면 업데이트
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        // 가장 가까운 적이 있을 경우
        if (closestEnemy != null)
        {
            // 타워의 위치에서 적의 위치로 향하는 방향 벡터 계산
            Vector3 direction = closestEnemy.transform.position - transform.position;

            // 방향 벡터를 기반으로 각도 계산 (Mathf.Atan2 사용)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 로그로 각도와 적의 유무 확인
            Debug.Log("각도: " + angle);
            Debug.Log("적을 찾음");

            // 타워가 정확히 적을 바라보게 하려면 angle을 90도 더해줍니다.
            angle += 270f;

            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
        else
        {
            // 적을 찾지 못한 경우 로그 출력
            Debug.Log("적을 찾지 못함");

            // 탐지 범위 내에 적이 없으면 초기 회전값으로 회전합니다.
            transform.rotation = initialRotation;
        }
    }
}
