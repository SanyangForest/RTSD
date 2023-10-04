using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen; // ��üȭ���� ���� ������ �̹���
    [SerializeField]
    private float maxHp = 20;
    private float currentHp;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        // ���� ü���� damage��ŭ ����
        currentHp -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0�� �Ǹ� ���ӿ���
        if (currentHp < 0)
        {

        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ��üȭ�� ũ��� ��ġ�� imageScreen �� ������ color ������ ����
        // imageScreen ������ 40%�� ����
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        // ������ 0%�� �ɶ����� ����
        while ( color.a >= 0.0f )
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;

        }
    }
}