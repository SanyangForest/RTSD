using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuplication : MonoBehaviour
{
    public bool IsBuildTower { set; get; } //타일에 타워가 건설되어 있는지 검사하는 변수

    private void Awake()
    {
        IsBuildTower = false;
    }
}
