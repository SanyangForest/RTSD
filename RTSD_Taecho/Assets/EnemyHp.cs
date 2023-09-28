using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float MaxHp;             // 최대 체력
    private float CurrentHp;         // 현재 체력
    private bool IsDie = false;     // 적이 사망상태이면 IsDie를 true로 설정
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        CurrentHp = MaxHp;                    // 현재 체력을 최대 체력과 같게 설정 
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // 현재 적의 상태가 사망 상태이면 아래 코드를 실행 하지 않는다.
        if (IsDie == true) return;

        // 현재 체력을 Damage만큼 감소
        CurrentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 적 체력이 0이하 = 적 캐릭터 사망
        if (CurrentHp <= 0)
        {
            IsDie = true;
            // 적 캐릭터 사망
            // enemy.OnDie();  < 나중에 주석처리 빼야하는 부분 현재 Enemy에 OnDie 에 대한 정의가 포함되어 있지 않음 작업 도중에 꺼야해서 커밋해서 올리려고 주석처리함 
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;

        // 적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.05f);

        // 적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
