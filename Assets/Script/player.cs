using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;

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

    [Header("Animation Controller")]
    //player ready
    public RuntimeAnimatorController player_ready_controller;
    //player idle sword
    public RuntimeAnimatorController player_idle_left_sword_controller;
    public RuntimeAnimatorController player_idle_right_sword_controller;
    //player run
    public RuntimeAnimatorController player_run_left_controller;
    public RuntimeAnimatorController player_run_right_controller;
    //player attack
    public RuntimeAnimatorController player_attack_right_controller;
    public RuntimeAnimatorController player_attack_left_controller;

    private Animator animator;

    [Header("Animation Duration")]
    private float player_attack_duration = 0.4f;

    void Start()
    {
        animator = GetComponent<Animator>();

        hitbox.SetActive(true);
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if(doAttack == true) return;

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

        //player가 바라보고있는 방향 판별
        if (moveDirection.x == 0)
        {
            WatchAnimation();
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

        //공격기능, 공격 히트박스 객체 형성
        if (Input.GetKeyDown(KeyCode.A))
        {
            doAttack = true;
            StartCoroutine(Attack()); //공격 애니메이션 처리 함수
            SpawnHitbox(); //hitbox생성 & 제거 함수
        }
    }

    //플레이어가 바라보는 방향에 따른 애니메이션 적용
    void WatchAnimation()
    {
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
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;

        if(progress >= 1f)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;
            //animator.runtimeAnimatorController = player_idle_controller;
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
        Debug.Log(hitbox_pos);

        //GameObject spawned_hitbox_prefab = Instantiate(hitbox, hitbox_pos, Quaternion.identity);
        //Destroy(spawned_hitbox_prefab, 0.5f);
    }
}
