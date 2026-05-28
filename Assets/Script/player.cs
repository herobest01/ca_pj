using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Stat")]
    public int hp = 3;

    [Header("Health")]
    public GameObject health_1;
    public GameObject health_2;
    public GameObject health_3;
    public GameObject health_bad_1;
    public GameObject health_bad_2;
    public GameObject health_bad_3;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;

    [Header("Attack Hitbox")]
    public Vector3 hitbox_pos; //hitbox 생성 위치

    //hitbox 방향, default: right
    public Vector3 hitbox_scale;
    public Vector3 hitbox_scale_flip;

    public GameObject hitbox;

    public bool spawn_hitbox = false;

    private SpriteRenderer spriteRenderer;

    private bool isJumping = false;
    private float jumpTimer = 0f;

    private Vector3 startPosition;

    private bool isMovingRight = false;

    private bool isWatchRight = true;

    private bool doAttack = false;
    //private float attackDuration = 1f;
    //private float attackTimer = 0f;

    //적과 플레이어 사이 간격에 의한 공격처리를 위한 변수
    private Transform enemy1_target;

    [Header("Animation Controller: ready")]
    //player ready
    public RuntimeAnimatorController player_ready_controller;

    [Header("Animation Controller: idle sword")]
    //player idle sword
    public RuntimeAnimatorController player_idle_left_sword_controller;
    public RuntimeAnimatorController player_idle_right_sword_controller;

    [Header("Animation Controller: run")]
    //player run
    public RuntimeAnimatorController player_run_left_controller;
    public RuntimeAnimatorController player_run_right_controller;

    [Header("Animation Controller: jump")]
    //player jump
    public RuntimeAnimatorController player_jump_up_left;
    public RuntimeAnimatorController player_jump_up_right;
    public RuntimeAnimatorController player_jump_down_left;
    public RuntimeAnimatorController player_jump_down_right;

    [Header("Animation Controller: attack")]
    //player attack
    public RuntimeAnimatorController player_attack_right_controller;
    public RuntimeAnimatorController player_attack_left_controller;

    private Animator animator;

    [Header("Animation Duration")]
    private float player_attack_duration = 0.4f;

    //camera.cs
    public main_camera camera;

    void Start()
    {
        animator = GetComponent<Animator>();

        //체력 초기 설정
        health_bad_1.SetActive(false);
        health_bad_2.SetActive(false);
        health_bad_3.SetActive(false);

        //히트박스 확인을 위한 코드
        hitbox.SetActive(false);

        //플레이어 공격당했을 때 효과 트리거 설정
        camera.isAttacked = true;
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if(doAttack == true) return;

        //체력 UI
        HealthShow();

        //c를 입력해서 체력확인
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log(hp);
        }

        // 캐릭터 이동 & animator
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1f;
            isWatchRight = false;
            RunAnimator();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1f;
            isMovingRight = true;
            isWatchRight = true;
            RunAnimator();
        }

        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        //점프기능
        if(Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            StartJump();
        }
        if (isJumping)
        {
            UpdateJump();
        }

        //player가 바라보고있는 방향 판별
        if (moveDirection.x == 0)
        {
            WatchAnimation();
        }

        //공격기능, 공격 히트박스 객체 형성
        if (Input.GetKeyDown(KeyCode.A))
        {
            doAttack = true;
            StartCoroutine(Attack()); //공격 애니메이션 처리 함수
            SpawnHitbox(); //hitbox생성 & 제거 함수
        }
    }

    void HealthShow()
    {
        switch (hp)
        {
            case 2:
            health_3.SetActive(false); 
            health_bad_3.SetActive(true);
            if (camera.isAttacked == true)
                {
                    camera.PlayerAttacked();
                    camera.isAttacked = false;
                }
            return;

            case 1:
            health_2.SetActive(false);
            health_bad_2.SetActive(true);
            if(camera.isAttacked != true)
                {
                    camera.PlayerAttacked();
                    camera.isAttacked = true;
                }
            return;

            case 0:
            health_1.SetActive(false);
            health_bad_1.SetActive(true);
            if (camera.isAttacked == true)
                {
                    camera.PlayerAttacked();
                    camera.isAttacked = false;
                }
            return;
        }
    }

    //플레이어가 바라보는 방향에 따른 애니메이션 적용
    void WatchAnimation()
    {
        if(isJumping == true)
        {
            return;
        }
        

        if(isWatchRight == true)
        {
            animator.runtimeAnimatorController = player_idle_right_sword_controller;
            //Debug.Log("watchanimation/right");
        }
        else if(isWatchRight == false)
        {
            animator.runtimeAnimatorController = player_idle_left_sword_controller;
            //Debug.Log("watchanimation/left");
        }
        else return;
    }

    //달리기 애니메이터
    void RunAnimator()
    {
        if(isWatchRight == true)
        {
            animator.runtimeAnimatorController = player_run_right_controller;
        }
        else
        {
            animator.runtimeAnimatorController = player_run_left_controller;
        }
    }

    // 점프시 변경사항
    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;

        if (isWatchRight == true)
        {
            animator.runtimeAnimatorController = player_jump_up_right;
        }
        else
        {
            animator.runtimeAnimatorController = player_jump_up_left;
        }
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;

        if(progress >= 1f)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;
            if (isWatchRight == true)
            {
                animator.runtimeAnimatorController = player_jump_down_right;
            }
            else
            {
                animator.runtimeAnimatorController = player_jump_down_left;
            }
        }
        else
        {
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(transform.position.x, startPosition.y + height, transform.position.z);
        }
    }

    //공격 관련 함수
    IEnumerator Attack()
    {   
        if(doAttack == true){
            if(isWatchRight == true)
            {
                animator.runtimeAnimatorController = player_attack_right_controller;

                yield return null;

                animator.Play(0);
                //Debug.Log("animator.Play시작");

                yield return new WaitForSeconds(player_attack_duration);

                animator.runtimeAnimatorController = player_idle_right_sword_controller;

                yield return null;
                
                animator.Play(0);
                //Debug.Log("animator.idle");

            }
            else if(isWatchRight == false)
            {
                animator.runtimeAnimatorController = player_attack_left_controller;

                yield return null;

                animator.Play(0);

                yield return new WaitForSeconds(player_attack_duration);

                animator.runtimeAnimatorController = player_idle_left_sword_controller;

                yield return null;
                
                animator.Play(0);
            }

            doAttack = false;
        }
    }

    public void SpawnHitbox()
    {
        spawn_hitbox = true;
        hitbox_scale = hitbox.transform.localScale;
        
        if(isWatchRight == true)
        {
            hitbox_pos = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.2f, 0f);

            GameObject spawned_hitbox_prefab = Instantiate(hitbox, hitbox_pos, Quaternion.identity);
            Destroy(spawned_hitbox_prefab, 0.5f);
        }
        else if(isWatchRight == false)
        {
            hitbox_scale_flip = new Vector3(hitbox_scale.x * -1f, hitbox_scale.y, 1f);
            hitbox_pos = new Vector3(transform.position.x - 0.5f, transform.position.y + 0.1f, 0f);

            GameObject spawned_hitbox_prefab = Instantiate(hitbox, hitbox_pos, Quaternion.identity);
            spawned_hitbox_prefab.transform.localScale = hitbox_scale_flip; //hitbox scale을 hit_box_flip에 있는 속성으로 변경
            Destroy(spawned_hitbox_prefab, 0.5f);
        }

        //GameObject spawned_hitbox_prefab = Instantiate(hitbox, hitbox_pos, Quaternion.identity);
        //Destroy(spawned_hitbox_prefab, 0.5f);
    }
}

//lorem