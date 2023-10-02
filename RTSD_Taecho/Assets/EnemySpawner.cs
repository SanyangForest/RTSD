using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefabs; // �� ������
    [SerializeField]
    private GameObject enemyHpSliderPrefab; // �� ü���� ��Ÿ���� Slider Ui ������
    [SerializeField]
    private Transform  canvasTransform;     // Ui�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    private float SpawnTime; // �� ���� �ֱ�
    [SerializeField]
    private Transform[] WayPoints; // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHp playerHp;     // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold; // �÷��̾��� ��� ������Ʈ 
    private List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������   EnemySpawner ���� �ϱ� ������ Set �� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;  

    private void Awake()
    {
        // �� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate (EnemyPrefabs);            // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();                // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(WayPoints);                                   // WayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);

            SpawnEnemyHpSlider(clone);

            yield return new WaitForSeconds(SpawnTime);               // SpawnTime �ð� ���� ���
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy,int gold)
    {
        // ���� ��ǥ�������� �������� �� 
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ü�� -1 
            playerHp.TakeDamage(1);
        }
        // ���� �÷��̾��� �߻�ü���� ������� ��
        else if (type == EnemyDestroyType.Kill)
        {
            // ���� ������ ���� ��� �� ��� ȹ��
            playerGold.CurrentGold += gold;
        }

        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHpSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider Ui ����
        GameObject sliderclone = Instantiate(enemyHpSliderPrefab);
        // Slider Ui ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        sliderclone.transform.SetParent (canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1,1,1)�� ����
        sliderclone.transform.localScale = Vector3.one;

        // Slider Ui�� �i�ƴٴ� ����� �������� ����
        sliderclone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider Ui�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderclone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}