using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int WayPointCount;              // �̵� ��� ����
    private Transform[] Waypoints;          // �̵� ��� ����
    private int CurrentIndex = 0;           // ���� ��ǥ���� �ε���
    private Movement2D Movement2D;          // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner;      // ���� ������ ������ ���� �ʰ� EnemySpawner�� �˷��� ���� 
    [SerializeField]
    private int gold = 10;                  // �� ����� ȹ�� ������ ���

    public void Setup(Transform[] Waypoints)
    {
        Movement2D = GetComponent<Movement2D>();

        // �� �̵� ��� WayPoints ���� ����
        WayPointCount = Waypoints.Length;
        this.Waypoints = new Transform[WayPointCount];
        this.Waypoints = Waypoints;

        // ���� ��ġ�� ù��° Waypoint ��ġ�� ����
        transform.position = Waypoints[CurrentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            // �� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            // ���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * Movement2D.moveSpeed���� ���� �� if ���ǹ� ����
            // Movement2D.moveSpeed�� �����ִ� ������ �ӵ��� ������ �� ������ �ȿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, Waypoints[CurrentIndex].position) < 0.02f * Movement2D.moveSpeed)
            {
                // ���� �̵� ���� ����
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // ���� �̵��� WayPoints�� �����ִٸ�
        if (CurrentIndex < WayPointCount - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = Waypoints[CurrentIndex].position;
            // �̵� ���� ���� => ���� ��ǥ���� (WayPoints)
            CurrentIndex++;
            Vector3 direction = (Waypoints[CurrentIndex].position - transform.position).normalized;
            Movement2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ WayPoints�̸�
        else
        {
            // ��ǥ������ �����ؼ� ����� ���� ���� ���� �ʵ��� 
            gold = 0;
            // �� ������Ʈ ����
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    { 
        enemySpawner.DestroyEnemy(type, this, gold);
    }
 } 