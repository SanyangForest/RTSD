using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatRandomTower : MonoBehaviour
{
    public GameObject[] towerPrefabs; // 다양한 타워 프리팹을 저장할 배열
    private GameObject currentTower; // 현재 생성된 타워를 저장할 변수
    //private float spawnYPosition = 0f; // 타워의 초기 y 좌표
    private GameObject clickedTower; // 클릭된 타워를 기억하기 위한 변수


    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField] PlayerGold gold;
    [SerializeField] TowerTemplate towerTemplate;

    private int level = 0;
    private PlayerGold playerGold;
    private SpriteRenderer spriteRenderer;

    private bool isOnBuild = false;
    private GameObject followTowerImage = null;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReadyToSpawnTower();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MergeTowers();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    public void ReadyToSpawnTower()
    {
        if (isOnBuild == true)  // 버튼을 중복으로 누르지 못하게 막는 장치
        {
            return;
        }

        if (20 > gold.CurrentGold) // 굳이 towerTemplate.weapon[0].cost 안써도 됨. 어차피 20G 고정임
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
            systemTextViewer.PrintText(SystemType.Money);
            Debug.Log("돈 부족");
            return;
        }
        Debug.Log("건설 가능");
        isOnBuild = true;

        // 마우스 따라다니는 이미지 생성
        followTowerImage = Instantiate(towerTemplate.followImage);

        // 타워 건설 취소 코루틴 시작
        StartCoroutine("OnTowerCancleSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        CheckDuplication check = tileTransform.GetComponent<CheckDuplication>();

        if(isOnBuild == false) // 타워 건설 버튼을 누를 때만 건설 가능
        {
            Debug.Log("타워 건설 버튼을 눌러주십시오");
            return;
        }

        if(check.IsBuildTower == true)  // 중복 체크
        {
            Debug.Log("이미 타워가 설치된 타일");
            return;
        }

        check.IsBuildTower = true;

        // 랜덤한 타워를 생성
        int randomIndex = Random.Range(0, towerPrefabs.Length);
        GameObject selectedTowerPrefab = towerPrefabs[randomIndex];
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        isOnBuild = false; // 설치됐으면 다시 false로 바꿔서 버튼을 클릭해야 설치되게 함

        // 타워를 생성하고 이름과 태그를 설정
        GameObject newTower = Instantiate(selectedTowerPrefab, tileTransform.position, Quaternion.identity);
        string towerName = selectedTowerPrefab.name;
        newTower.name = towerName;
        newTower.tag = "T_L_1"; // 고정된 태그 "T_L_1"로 설정

        // 현재 타워를 업데이트하고 spawnYPosition 증가
        currentTower = newTower;

        gold.CurrentGold -= 20;  // 타워 20원
        Destroy(followTowerImage);
        StopCoroutine("OnTowerCancleSystem");
    }

    private IEnumerator OnTowerCancleSystem()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnBuild = false;
                // 마우스 따라다니는 이미지도 삭제
                Destroy(followTowerImage);
                break;
            }

            yield return null;
        }
    }
    public void HandleClick()
    {
        // 마우스 클릭 좌표를 월드 좌표로 변환
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2D 레이캐스트를 사용하여 클릭한 오브젝트를 찾음
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            // 클릭한 오브젝트의 태그가 "T_L_1"부터 "T_L_5" 중 하나와 일치하는지 확인
            if (hit.collider.CompareTag("T_L_1") || hit.collider.CompareTag("T_L_2") || hit.collider.CompareTag("T_L_3") || hit.collider.CompareTag("T_L_4") || hit.collider.CompareTag("T_L_5"))
            {
                // 클릭된 타워로 설정
                clickedTower = hit.collider.gameObject;
                Debug.Log("Clicked Tower: " + clickedTower.name);
            }
        }
    }


    public void MergeTowers()
    {
        // 클릭된 타워가 유효한지 확인
        if (clickedTower != null)
        {
            // 클릭된 타워의 이름과 태그를 가져옵니다.
            string clickedName = clickedTower.name;
            string clickedTag = clickedTower.tag;

            // 같은 이름과 태그를 가진 타워를 찾아서 합성
            GameObject[] allTowers = GameObject.FindGameObjectsWithTag(clickedTag);
            int mergeCount = 0;

            foreach (GameObject otherTower in allTowers)
            {
                if (otherTower != clickedTower && otherTower.name == clickedName)
                {
                    // 합병될 타워의 이름을 설정
                    string mergedName = clickedName;

                    // 합병된 타워를 생성하고 이름과 태그를 설정
                    GameObject mergedTower =
                        Instantiate(clickedTower, clickedTower.transform.position, Quaternion.identity);
                    mergedTower.name = mergedName;

                    // 태그를 업데이트하여 "T_L_(n+1)"로 설정
                    int lastUnderscoreIndex = clickedTag.LastIndexOf('_');
                    if (lastUnderscoreIndex != -1)
                    {
                        string tagCounterStr = clickedTag.Substring(lastUnderscoreIndex + 1);
                        if (int.TryParse(tagCounterStr, out int tagCounter))
                        {
                            tagCounter++;
                            mergedTower.tag = "T_L_" + tagCounter;
                        }
                    }

                    // 기존 타워 삭제
                    Destroy(clickedTower);
                    Destroy(otherTower);

                    mergeCount++;

                    // 2개의 타워를 합성하면 종료
                    if (mergeCount >= 2)
                    {
                        break;
                    }
                }
            }

            // 클릭된 타워 초기화
            clickedTower = null;
        }
    }
    public bool Upgrade()
    {
        if (playerGold.CurrentGold < 10)
        {
            return false;
        }

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        playerGold.CurrentGold -= 10;

        return true;
    }
}
