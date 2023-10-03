using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttt : MonoBehaviour
{
    public float speed = 1f; // 이동 속도

    void Update()
    {
        // 게임 오브젝트를 왼쪽으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
