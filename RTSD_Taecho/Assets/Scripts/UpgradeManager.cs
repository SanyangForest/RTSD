using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    public CreatRandomTower towerSpawner; 
    public void OnClickEventTowerUpgrade()
    {
        bool isSuccess = towerSpawner.Upgrade();

        if (isSuccess == true)
        {
            //UpdateTowerData();
        }
        else
        {
            // 타워 업그레이드에 필요한 비용이 부족하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

}
