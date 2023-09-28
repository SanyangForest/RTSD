using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float MaxHp;             // �ִ� ü��
    private float CurrentHp;         // ���� ü��
    private bool IsDie = false;     // ���� ��������̸� IsDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        CurrentHp = MaxHp;                    // ���� ü���� �ִ� ü�°� ���� ���� 
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 ���� ���� �ʴ´�.
        if (IsDie == true) return;

        // ���� ü���� Damage��ŭ ����
        CurrentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // �� ü���� 0���� = �� ĳ���� ���
        if (CurrentHp <= 0)
        {
            IsDie = true;
            // �� ĳ���� ���
            // enemy.OnDie();  < ���߿� �ּ�ó�� �����ϴ� �κ� ���� Enemy�� OnDie �� ���� ���ǰ� ���ԵǾ� ���� ���� �۾� ���߿� �����ؼ� Ŀ���ؼ� �ø����� �ּ�ó���� 
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        // ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);

        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
