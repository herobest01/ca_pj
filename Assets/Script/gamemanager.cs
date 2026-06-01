using UnityEngine;
using TMPro;

public class gamemanager : MonoBehaviour
{
    [Header("Stage Background")]
    public GameObject stage1_background;
    public GameObject stage2_background;
    public GameObject stage3_background;

    [Header("Enemys")]
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    //public GameObject enemy3;

    [Header("Stage2 Death Count")]
    public TextMeshProUGUI stage2_deathcount_text;

    private int now_stage;
    private bool enemy2_isSummon = true;
    public int enemy2_left = 15;


    void Start()
    {
        now_stage = 1;

        stage2_background.SetActive(false);
        stage3_background.SetActive(false);

        stage2_deathcount_text.text = "";
    }

    void Update()
    {
        EnemyAI enemy1_sc = enemy1.GetComponent<EnemyAI>();

        //---------- 1스테이지 ----------
        //enemy1 체력확인
        if(now_stage == 1)
        {
            if(enemy1_sc.hp <= 0)
            {
                Stage1toStage2();
            }
        }
        //---------- 2스테이지 ----------
        else if(now_stage == 2)
        {
            enemy2 enemy2_sc = enemy2.GetComponent<enemy2>();

            Stage2Deathcount();

            //소환시작
            if(enemy2_isSummon == true)
            {
                enemy2_sc.Summon();
            }
            enemy2_isSummon = false;
        }

        //변수 확인
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Now stage: " + now_stage);
        }

        //바로가기
        if (Input.GetKey(KeyCode.I))
        {
            now_stage = 1;
        }
        else if (Input.GetKey(KeyCode.O))
        {
            now_stage = 2;
        }
        else if (Input.GetKey(KeyCode.P))
        {
            now_stage = 3;
        }
    }

    //stage1 -> stage2
    void Stage1toStage2()
    {
        player player_sc = player.GetComponent<player>();

        now_stage++;
        player_sc.hp = 3;

        //비활성화 요소
        enemy1.SetActive(false);
        stage1_background.SetActive(false);

        //활성화 요소
        stage2_background.SetActive(true);
    }

    //stage2 -> stage3
    void Stage2toStage3()
    {
        now_stage++;

        //비활성화 요소
        //enemy2.SetActive(false);
        stage2_background.SetActive(false);

        //활성화 요소
        stage3_background.SetActive(true);
    }

    //stage2 데스카운트 표시
    void Stage2Deathcount()
    {
        enemy2 enemy2_sc = enemy2.GetComponent<enemy2>();

        stage2_deathcount_text.text = "X "+ enemy2_left;
    }
}
