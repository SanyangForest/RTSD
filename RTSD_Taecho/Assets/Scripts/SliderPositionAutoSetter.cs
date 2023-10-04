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
        // Slider Ui가 쫒아다닐 target 설정
        targetTramsform = target;
        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 쫒아다닐 대상이 사라지면 Slider Ui도 삭제
        if (targetTramsform == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트의 위치가 갱신된 이후에 Slider Ui도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 호출한다

        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTramsform.position);
        // 화면 내에서 좌표 + distance 만큼 떨어진 위치를 Slider Ui의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }

}

