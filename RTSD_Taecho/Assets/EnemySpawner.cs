using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefabs; // �� ������
    [SerializeField]
    private float SpawnTime; // �� ���� �ֱ�
    [SerializeField]
    private Transform[] WayPoints; // ���� ���������� �̵� ���

    private void Awake()
    {
        // �� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate (EnemyPrefabs);            // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();                // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(WayPoints);                                   // WayPoint ������ �Ű������� Setup() ȣ��

            yield return new WaitForSeconds(SpawnTime);               // SpawnTime �ð� ���� ���
        }
    }
}