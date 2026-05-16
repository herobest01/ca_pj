using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;

    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private bool isMovingRight = false;
    private bool isWatchRight = true;

    [Header("Animation Controller")]
    //player ready
    public RuntimeAnimatorController player_ready_controller;
    //player idle
    public RuntimeAnimatorController player_idle_left_controller;
    public RuntimeAnimatorController player_idle_right_controller;
    //player run
    public RuntimeAnimatorController player_run_left_controller;
    public RuntimeAnimatorController player_run_right_controller;
    //player attack
    //public RuntimeAnimatorController player_attack_right_controller;
    //public RuntimeAnimatorController player_attack_left_controller;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

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

        if (moveDirection.x == 0)
        {
            if(isWatchRight == true)
            {
                animator.runtimeAnimatorController = player_idle_right_controller;
            }
            else if (isWatchRight == false)
            {
                animator.runtimeAnimatorController = player_idle_left_controller;
            }
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
}
