using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; // ���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1; // ���� ���̺� �ε��� 

    public void StartWave()
    {
        // ���� �ʿ� ���� ����, Wave�� ���� ������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // �ε����� ������ -1 �̱� ������ ���̺� �ε��� ������ ���� ���� ��
            currentWaveIndex++;
            // EnemySpawner�� startWave() �Լ� ȣ�� . ���� ���̺� ���� ���� 
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}
[System.Serializable]
    public struct Wave
    {
        public float spawnTime;           // ���� ���̺� �� ���� �ֱ�
        public int maxEnemyCount;         // ���� ���̺� �� ���� ����
        public GameObject[] enemyPrefabs; // ���� ���̺� �� ���� ����
    }

