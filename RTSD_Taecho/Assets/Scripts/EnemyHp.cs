using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float maxHp;             // �ִ� ü��
    private float currentHp;         // ���� ü��
    private bool isDie = false;     // ���� ��������̸� IsDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;                    // ���� ü���� �ִ� ü�°� ���� ���� 
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 ���� ���� �ʴ´�.
        if (isDie == true) return;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

        // ���� ü���� Damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        Debug.Log("HP-1");

        // �� ü���� 0���� = �� ĳ���� ���
        if (currentHp <= 0)
        {
            isDie = true;
            // �� ĳ���� ���
            enemy.OnDie(EnemyDestroyType.Kill);  

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
