using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuplication : MonoBehaviour
{
    public bool IsBuildTower { set; get; } //Ÿ�Ͽ� Ÿ���� �Ǽ��Ǿ� �ִ��� �˻��ϴ� ����

    private void Awake()
    {
        IsBuildTower = false;
    }
}
