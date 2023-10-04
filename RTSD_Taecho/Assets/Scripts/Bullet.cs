using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target; // 총알의 대상 (적 캐릭터)
    public float speed = 15f; // 총알 이동 속도
    public int damage = 10; // 총알의 피해량

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            // 대상이 없으면 총알 파괴
            Destroy(gameObject);
            return;
        }

        // 대상 쪽으로 이동
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            // 대상에 도달했을 때
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // 대상에게 피해를 입히고 총알 파괴
        target.GetComponent<Enemy>().TakeDamage(damage);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알이 충돌한 대상이 Enemy 태그를 가지고 있는지 확인
        if (collision.CompareTag("Enemy"))
        {
            // 대상이 Enemy 태그를 가진 오브젝트라면 해당 오브젝트의 Enemy 스크립트를 가져옴
            Enemy enemy = collision.GetComponent<Enemy>();

            // Enemy 스크립트가 존재하면 피해를 입힘
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 총알을 삭제
            Destroy(gameObject);
        }

    }
}