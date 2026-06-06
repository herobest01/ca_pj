using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    [Header("Stage Background")]
    public GameObject stage1_background;
    public GameObject stage2_background;
    public GameObject stage3_background;

    [Header("Tilemap in Stage3")]
    public GameObject stage3_tilemap;

    [Header("Panel")]
    public GameObject panel_main;
    public GameObject panel_guide;
    public GameObject panel_1to2;
    public GameObject panel_2to3;
    public GameObject panel_gameover;
    public GameObject panel_gameclear;

    [Header("Button in panel_main")]
    public Button button_guide;
    public Button button_quit;

    [Header("Button in panel_guide")]
    public Button button_start;

    [Header("Button in panel_1to2")]
    public Button button_check_1to2;

    [Header("Button in panel_2to3")]
    public Button button_check_2to3;

    [Header("Button in panel_gameover")]
    public Button button_retry;
    public Button button_quit_gameover;

    [Header("Button in panel_gameclear")]
    public Button button_quit_gameclear;

    [Header("Health UI")]
    public GameObject health_1;
    public GameObject health_2;
    public GameObject health_3;
    public GameObject health_bad_1;
    public GameObject health_bad_2;
    public GameObject health_bad_3;

    [Header("Enemys")]
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    [Header("Canvas Text")]
    public TextMeshProUGUI canvas_text;

    private int now_stage;
    private bool enemy2_isSummon = true;

    private bool isPanelOn = false;
    private bool isCheck = false;
    public bool isGameover = false;


    void Start()
    {
        now_stage = 1;

        panel_main.SetActive(true);
        panel_guide.SetActive(false);
        panel_1to2.SetActive(false);
        panel_2to3.SetActive(false);
        panel_gameover.SetActive(false);
        panel_gameclear.SetActive(false);

        player.SetActive(false);
        enemy1.SetActive(false);
        enemy3.SetActive(false);

        stage1_background.SetActive(true);
        stage2_background.SetActive(false);
        stage3_background.SetActive(false);

        stage3_tilemap.SetActive(false);

        canvas_text.text = "";

        //button관련
        button_guide.onClick.AddListener(GameGuide);
        button_quit.onClick.AddListener(GameQuit);
        button_start.onClick.AddListener(GameStart);
        button_check_1to2.onClick.AddListener(CheckIn1to2);
        button_check_2to3.onClick.AddListener(CheckIn2to3);
        button_quit.onClick.AddListener(GameQuit);
        button_retry.onClick.AddListener(GameRetry);
        button_quit_gameclear.onClick.AddListener(GameQuit);

        Enemy2 enemy2_sc = enemy2.GetComponent<Enemy2>();
        enemy2_sc.gamemanager = this;
    }

    void Update()
    {
        //---------- 1스테이지 ----------
        if(now_stage == 1)
        {
            EnemyAI enemy1_sc = enemy1.GetComponent<EnemyAI>();
            canvas_text.text = "Enemy HP: " + enemy1_sc.hp;

            if(enemy1_sc.hp <= 0 && !isPanelOn)
            {
                now_stage = 2;

                enemy1.SetActive(false);

                panel_1to2.SetActive(true);

                isPanelOn = true;
                Stage1toStage2();
            }
        }
        //---------- 2스테이지 ----------
        else if(now_stage == 2)
        {
            Enemy2 enemy2_sc = enemy2.GetComponent<Enemy2>();

            if(isPanelOn == true)
            {
               return; 
            }
            else if(isPanelOn == false && isCheck == true)
            {
                enemy2.SetActive(true);

                Stage2Deathcount();

                //소환시작
                if(enemy2_isSummon == true)
                {
                    enemy2_sc.Summon();
                }
                enemy2_isSummon = false;
            }

            if(Enemy2.death_count >= 15)
            {
                isPanelOn = true;
                enemy2.SetActive(false);
                Stage2toStage3();
                panel_2to3.SetActive(true);
            }
        }
        //---------- 3스테이지 ----------
        else if(now_stage == 3)
        {
            Enemy3 enemy3_sc = enemy3.GetComponent<Enemy3>();
            canvas_text.text = "Enemy HP: " + enemy3_sc.hp;

            if(isPanelOn == true)
            {
                return;
            }
            else if(isPanelOn == false && isCheck == true)
            {
                enemy3.SetActive(true);
            }

            if(enemy3_sc.hp <= 0)
            {
                isPanelOn = true;
                GameClear();
            }
        }

        player player_sc = player.GetComponent<player>();

        if(player_sc.hp <= 0 && isGameover == false)
        {
            isGameover = true;
            Gameover();
        }

        /*바로가기
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
        */
    }

    //stage1 -> stage2
    void Stage1toStage2()
    {
        player player_sc = player.GetComponent<player>();
        player_sc.hp = 3;

        //비활성화 요소
        stage1_background.SetActive(false);

        //활성화 요소
        stage2_background.SetActive(true);
    }

    //stage2 -> stage3
    void Stage2toStage3()
    {
        player player_sc = player.GetComponent<player>();
        
        now_stage = 3;
        player_sc.hp = 3;

        //비활성화 요소
        stage2_background.SetActive(false);

        //활성화 요소
        stage3_background.SetActive(true);
        stage3_tilemap.SetActive(true);
    }

    //stage2 데스카운트 표시
    void Stage2Deathcount()
    {
        canvas_text.text = $"X {15 - Enemy2.death_count}";
    }

    //panel_main -> panel_guide
    void GameGuide()
    {
        panel_main.SetActive(false);
        panel_guide.SetActive(true);
    }

    //panel_1to2에 위치한 버튼
    void CheckIn1to2()
    {
        player player_sc = player.GetComponent<player>();
        Vector3 i = new Vector3(1f, -3.2f, 0f);

        panel_1to2.SetActive(false);

        enemy2.SetActive(true);

        player_sc.transform.position = i;
        
        isPanelOn = false;
        isCheck = true;
    }

    //panel_1to2에 위치한 버튼
    void CheckIn2to3()
    {
        player player_sc = player.GetComponent<player>();
        Vector3 i = new Vector3(5.75f, -2.94f, 0f);

        now_stage = 3;

        panel_2to3.SetActive(false);

        enemy3.SetActive(true);

        player_sc.transform.position = i;
        
        isPanelOn = false;
        isCheck = true;
    }

    //게임오버처리
    void Gameover()
    {
        //player.SetActive(false);
        panel_gameover.SetActive(true);

        if(now_stage == 1)
        {
            enemy1.SetActive(false);
        }
        else if(now_stage == 2)
        {
            enemy2.SetActive(false);
        }
        else if(now_stage == 3)
        {
            enemy3.SetActive(false);
        }
    }

    //게임오버 재도전 버튼
    void GameRetry()
    {
        player player_sc = player.GetComponent<player>();
        player_sc.hp = 3;
        player.SetActive(true);
        panel_gameover.SetActive(false);

        isGameover = false;

        if(now_stage == 1)
        {
            EnemyAI enemy1_sc = enemy1.GetComponent<EnemyAI>();
            enemy1_sc.hp = 15;

            Vector3 player_i = new Vector3(-6.0f, -2.8f, 0f);
            Vector3 enemy1_i = new Vector3(3.8f, -2.8f, 0f);
            player.transform.position = player_i;
            enemy1.transform.position = enemy1_i;

            enemy1.SetActive(true);
        }
        else if(now_stage == 2)
        {
            GameObject[] enemy2_prefabs = GameObject.FindGameObjectsWithTag("Enemy2");
            Enemy2 enemy2_sc = enemy2.GetComponent<Enemy2>();

            foreach (GameObject i in enemy2_prefabs)
            {
                Destroy(i);
            }
            
            enemy2_sc.spawn_count = 15;
            Enemy2.death_count = 0;
            enemy2_sc.Summon();
        }
        else if(now_stage == 3)
        {
            Enemy3 enemy3_sc = enemy3.GetComponent<Enemy3>();
            enemy3_sc.hp = 40;
        }
    }

    void GameClear()
    {
        enemy3.SetActive(false);
        panel_gameclear.SetActive(true);
        
    }

    void GameQuit()
    {
        Application.Quit();
    }

    // panel_guide에 있는 '확인'버튼
    void GameStart()
    {
        panel_guide.SetActive(false);

        player.SetActive(true);
        enemy1.SetActive(true);

        Debug.Log("Start");
    }
}
