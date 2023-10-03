using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTramsform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider Ui�� �i�ƴٴ� target ����
        targetTramsform = target;
        // RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� �i�ƴٴ� ����� ������� Slider Ui�� ����
        if (targetTramsform == null)
        {
            Destroy(gameObject);
            return;
        }

        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider Ui�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�

        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTramsform.position);
        // ȭ�� ������ ��ǥ + distance ��ŭ ������ ��ġ�� Slider Ui�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }

}

