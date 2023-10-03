using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveSystem;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject EnemyPrefabs; // 적 프리팹
    //[SerializeField]
    //private float SpawnTime; // 적 생성 주기
    [SerializeField]
    private Transform[] wayPoints; // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHp playerHp;     // 플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold playerGold; // 플레이어의 골드 컴포넌트 
    private Wave currentWave;      // 현재 웨이브 정보
    private int currentEnemyCount;		// 현재 웨이브에 남아있는 적 숫자 (웨이브 시작시 max로 설정, 적 사망 시 -1)
    private List<Enemy> enemyList; // 현재 맵에 존재하는 모든 적의 정보

    public int CurrentEnemyCount => currentEnemyCount;
    public List<Enemy> EnemyList => enemyList;
     
    // 적의 생성과 삭제는   EnemySpawner 에서 하기 때문에 Set 은 필요 없다.

    private void Awake()
    {
        enemyList = new List<Enemy>();
        // 적 생성 코루틴 함수 생성
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        // 현재 웨이브의 최대 적 숫자를 저장
        currentEnemyCount = currentWave.maxEnemyCount;
        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        // 현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;
        // 현재 웨이브에서 생성되어야 하는 적의 숫자만큼 적을 생성하고 코루틴 종료
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // 웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고, 적 오브젝트 생성
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();  // 방금 생성된 적의 Enemy 컴포넌트

            // this는 나 자신 (자신의 EnemySpawner 정보)
            enemy.Setup(this, wayPoints);                     // wayPoint 정보를 매개변수 Setup() 호출
            enemyList.Add(enemy);                             // 리스트에 방금 생성된 적 정보 저장

            // 현재 웨이브에서 생성한 적의 숫자 +1 
            spawnEnemyCount++;
            // 각 웨이브마다 spawnTime이 다를 수 있기 때문에 현재 웨이브 currentWave의 spawnTime 사용
            yield return new WaitForSeconds(currentWave.spawnTime);
            // SpawnTime 시간 동안 대기
        }

        //while (true)
        //{
        //    //GameObject clone = Instantiate (EnemyPrefabs);            // 적 오브젝트 생성
        //    //Enemy enemy = clone.GetComponent<Enemy>();                // 방금 생성된 적의 Enemy 컴포넌트

        //    //enemy.Setup(WayPoints);                                   // WayPoint 정보를 매개변수로 Setup() 호출

        //    //yield return new WaitForSeconds(SpawnTime);               // SpawnTime 시간 동안 대기
        //}
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy,int gold)
    {
        // 적이 목표지점까지 도착했을 때 
        if (type == EnemyDestroyType.Arrive)
        {
            // 플레이어의 체력 -1 
            playerHp.TakeDamage(1);
        }
        // 적이 플레이어의 발사체에서 사망했을 때
        else if (type == EnemyDestroyType.Kill)
        {
            // 적의 종류에 따라 사망 시 골드 획득
            playerGold.CurrentGold += gold;
        }

        // 리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        Destroy(enemy.gameObject);
    }


}