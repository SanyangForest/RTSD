using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;
    public Transform firePoint; // 총알 발사 지점
    public GameObject bulletPrefab; // 총알 프리팹
    public float fireRate = 2f; // 발사 간격 (초 단위)
    public int damage = 10; // 총알 데미지

    private int level = 0;
    private PlayerGold playerGold;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // 코루틴을 사용하여 총알 자동 발사
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // 발사 간격 대기

            // 총알 발사
            FireBullet();
        }
    }

    private void FireBullet()
    {
        // 가장 가까운 "Enemy" 태그를 가진 오브젝트를 찾음
        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            // 총알을 발사 지점에서 생성
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 총알에 데미지 정보 설정
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.damage = damage;
            }

            // 총알을 가장 가까운 적에게 발사
            bulletComponent.Seek(closestEnemy.transform);

            // 발사 정보 로그 출력
            Debug.Log("Bullet fired at: " + closestEnemy.name);
        }
        else
        {
            Debug.Log("No target to shoot at.");
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public bool Upgrade()
    {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        return true;
    }
}