using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefabs; // 적 프리팹
    [SerializeField]
    private GameObject enemyHpSliderPrefab; // 적 체력을 나타내는 Slider Ui 프리팹
    [SerializeField]
    private Transform  canvasTransform;     // Ui를 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]
    private float SpawnTime; // 적 생성 주기
    [SerializeField]
    private Transform[] WayPoints; // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHp playerHp;     // 플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold playerGold; // 플레이어의 골드 컴포넌트 
    private List<Enemy> enemyList; // 현재 맵에 존재하는 모든 적의 정보

    // 적의 생성과 삭제는   EnemySpawner 에서 하기 때문에 Set 은 필요 없다.
    public List<Enemy> EnemyList => enemyList;  

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
            enemyList.Add(enemy);

            SpawnEnemyHpSlider(clone);

            yield return new WaitForSeconds(SpawnTime);               // SpawnTime 시간 동안 대기
        }
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

    private void SpawnEnemyHpSlider(GameObject enemy)
    {
        // 적 체력을 나타내는 Slider Ui 생성
        GameObject sliderclone = Instantiate(enemyHpSliderPrefab);
        // Slider Ui 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        sliderclone.transform.SetParent (canvasTransform);
        // 계층 설정으로 바뀐 크기를 다시 (1,1,1)로 설정
        sliderclone.transform.localScale = Vector3.one;

        // Slider Ui가 쫒아다닐 대상을 본인으로 설정
        sliderclone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider Ui에 자신의 체력 정보를 표시하도록 설정
        sliderclone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}