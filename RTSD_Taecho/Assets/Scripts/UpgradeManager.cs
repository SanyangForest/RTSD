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
            // Ÿ�� ���׷��̵忡 �ʿ��� ����� �����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

}
