using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveSystem;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject EnemyPrefabs; // �� ������
    //[SerializeField]
    //private float SpawnTime; // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints; // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHp playerHp;     // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold; // �÷��̾��� ��� ������Ʈ 
    private Wave currentWave;      // ���� ���̺� ����
    private int currentEnemyCount;		// ���� ���̺꿡 �����ִ� �� ���� (���̺� ���۽� max�� ����, �� ��� �� -1)
    private List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����

    public int CurrentEnemyCount => currentEnemyCount;
    public List<Enemy> EnemyList => enemyList;
     
    // ���� ������ ������   EnemySpawner ���� �ϱ� ������ Set �� �ʿ� ����.

    private void Awake()
    {
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ �Լ� ����
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        // ���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;
        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();  // ��� ������ ���� Enemy ������Ʈ

            // this�� �� �ڽ� (�ڽ��� EnemySpawner ����)
            enemy.Setup(this, wayPoints);                     // wayPoint ������ �Ű����� Setup() ȣ��
            enemyList.Add(enemy);                             // ����Ʈ�� ��� ������ �� ���� ����

            // ���� ���̺꿡�� ������ ���� ���� +1 
            spawnEnemyCount++;
            // �� ���̺긶�� spawnTime�� �ٸ� �� �ֱ� ������ ���� ���̺� currentWave�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime);
            // SpawnTime �ð� ���� ���
        }

        //while (true)
        //{
        //    //GameObject clone = Instantiate (EnemyPrefabs);            // �� ������Ʈ ����
        //    //Enemy enemy = clone.GetComponent<Enemy>();                // ��� ������ ���� Enemy ������Ʈ

        //    //enemy.Setup(WayPoints);                                   // WayPoint ������ �Ű������� Setup() ȣ��

        //    //yield return new WaitForSeconds(SpawnTime);               // SpawnTime �ð� ���� ���
        //}
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy,int gold)
    {
        // ���� ��ǥ�������� �������� �� 
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ü�� -1 
            playerHp.TakeDamage(1);
        }
        // ���� �÷��̾��� �߻�ü���� ������� ��
        else if (type == EnemyDestroyType.Kill)
        {
            // ���� ������ ���� ��� �� ��� ȹ��
            playerGold.CurrentGold += gold;
        }

        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }


}