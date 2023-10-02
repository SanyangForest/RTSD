using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int WayPointCount;              // 이동 경로 개수
    private Transform[] Waypoints;          // 이동 경로 정보
    private int CurrentIndex = 0;           // 현재 목표지점 인덱스
    private Movement2D Movement2D;          // 오브젝트 이동 제어
    private EnemySpawner enemySpawner;      // 적의 삭제를 본인이 하지 않고 EnemySpawner에 알려서 삭제 
    [SerializeField]
    private int gold = 10;                  // 적 사망시 획득 가능한 골드

    public void Setup(Transform[] Waypoints)
    {
        Movement2D = GetComponent<Movement2D>();

        // 적 이동 경로 WayPoints 정보 설정
        WayPointCount = Waypoints.Length;
        this.Waypoints = new Transform[WayPointCount];
        this.Waypoints = Waypoints;

        // 적의 위치를 첫번째 Waypoint 위치로 설정
        transform.position = Waypoints[CurrentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // 다음 이동 방향 설정
        NextMoveTo();

        while (true)
        {
            // 적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            // 적의 현재위치와 목표위치의 거리가 0.02 * Movement2D.moveSpeed보다 작을 때 if 조건문 실행
            // Movement2D.moveSpeed를 곱해주는 이유는 속도가 빠르면 한 프레임 안에 0.02보다 크게 움직이기 때문에
            // if 조건문에 걸리지 않고 경로를 탈출하는 오브젝트가 발생할 수 있기 때문이다.
            if (Vector3.Distance(transform.position, Waypoints[CurrentIndex].position) < 0.02f * Movement2D.moveSpeed)
            {
                // 다음 이동 방향 설정
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // 아직 이동할 WayPoints가 남아있다면
        if (CurrentIndex < WayPointCount - 1)
        {
            // 적의 위치를 정확하게 목표 위치로 설정
            transform.position = Waypoints[CurrentIndex].position;
            // 이동 방향 설정 => 다음 목표지점 (WayPoints)
            CurrentIndex++;
            Vector3 direction = (Waypoints[CurrentIndex].position - transform.position).normalized;
            Movement2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 WayPoints이면
        else
        {
            // 목표지점에 도달해서 사망할 때는 돈을 주지 않도록 
            gold = 0;
            // 적 오브젝트 삭제
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    { 
        enemySpawner.DestroyEnemy(type, this, gold);
    }
 } 