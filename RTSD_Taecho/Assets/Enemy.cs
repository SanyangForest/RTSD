using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int wayPointCount;              // �̵� ��� ����
    private Transform[] wayPoints;          // �̵� ��� ����
    private int currentIndex = 0;           // ���� ��ǥ���� �ε���
    private Movement2D movement2D;          // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner;      // ���� ������ ������ ���� �ʰ� EnemySpawner�� �˷��� ���� 
    [SerializeField]
    private int gold = 10;                  // �� ����� ȹ�� ������ ���
    public int startingHP = 100; // ���� ü��
    private int currentHP; // ���� ü��

    private void Start()
    {
        currentHP = startingHP; // ������ �� ���� ü���� �ִ� ü������ ����
    }

    public void Setup(EnemySpawner enemyspawner, Transform[] waypoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemyspawner;

        // �� �̵� ��� WayPoints ���� ����
        wayPointCount = waypoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = waypoints;

        // ���� ��ġ�� ù��° Waypoint ��ġ�� ����
        transform.position = waypoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            

            // ���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * Movement2D.moveSpeed���� ���� �� if ���ǹ� ����
            // Movement2D.moveSpeed�� �����ִ� ������ �ӵ��� ������ �� ������ �ȿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.moveSpeed)
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
        if (currentIndex < wayPointCount - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            // �̵� ���� ���� => ���� ��ǥ���� (WayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
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
    public void TakeDamage(int damage)
    {
        currentHP -= damage; // ���� ü�¿��� ���� ���ظ�ŭ ����

        // ���� ü���� 0 ���Ϸ� �������� ��� ó��
        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("�� ü��: " + currentHP); // ü���� ������ �� �α� ���
        }
    }
    private void Die()
    {
        // ���⿡�� ���� ��� ������ �����մϴ�.

        // �� ������Ʈ ����
        Destroy(gameObject);
    }
 } 