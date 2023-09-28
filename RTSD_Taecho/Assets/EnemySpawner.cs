using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefabs; // 적 프리팹
    [SerializeField]
    private float SpawnTime; // 적 생성 주기
    [SerializeField]
    private Transform[] WayPoints; // 현재 스테이지의 이동 경로

    private void Awake()
    {
        // 적 생성 코루틴 함수 생성
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate (EnemyPrefabs);            // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();                // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(WayPoints);                                   // WayPoint 정보를 매개변수로 Setup() 호출

            yield return new WaitForSeconds(SpawnTime);               // SpawnTime 시간 동안 대기
        }
    }
}