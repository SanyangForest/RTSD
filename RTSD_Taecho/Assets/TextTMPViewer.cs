using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textplayerHp;   // Text - TextMeshPro Ui [플레이어의 체력]
    [SerializeField]
    private TextMeshProUGUI textplayerGold; // Text - TextMeshpro Ui [플레이어의 골드]
    [SerializeField]
    private PlayerHp playerHp;              // 플레이어의 체력 정보
    [SerializeField]
    private PlayerGold playerGold;          // 플레이어의 골드 정보  

    private void Update()
    {
        textplayerHp.text = playerHp.CurrentHp + "/" + playerHp.MaxHp;
        textplayerGold.text = playerGold.CurrentGold.ToString();
    }
}
