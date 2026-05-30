using UnityEngine;

public class gamemanager : MonoBehaviour
{
    [Header("Stage Background")]
    public GameObject stage1_background;
    public GameObject stage2_background;
    public GameObject stage3_background;

    [Header("Enemys")]
    public GameObject enemy1;
    //public GameObject enemy2;
    //public GameObject enemy3;

    private int now_stage;


    void Start()
    {
        now_stage = 1;

        stage2_background.SetActive(false);
        stage3_background.SetActive(false);
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
            
        }

        //변수 확인
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Now stage: " + now_stage);
        }
    }

    //stage1 -> stage2
    void Stage1toStage2()
    {
        now_stage++;

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
}
