using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textplayerHp;   // Text - TextMeshPro Ui [�÷��̾��� ü��]
    [SerializeField]
    private TextMeshProUGUI textplayerGold; // Text - TextMeshpro Ui [�÷��̾��� ���]
    [SerializeField]
    private PlayerHp playerHp;              // �÷��̾��� ü�� ����
    [SerializeField]
    private PlayerGold playerGold;          // �÷��̾��� ��� ����  

    private void Update()
    {
        textplayerHp.text = playerHp.CurrentHp + "/" + playerHp.MaxHp;
        textplayerGold.text = playerGold.CurrentGold.ToString();
    }
}
