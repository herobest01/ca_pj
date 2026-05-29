using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 15;

    [Header("Target")]
    public string playerTag = "Player";
    private Transform target;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistance = 1.75f;

    [Header("Attack")]
    //공격관련수치
    public float attackCoolTime = 1f;
    public int damage = 1;
    private float lastAttackTime;
    private float attackDuration = 1f;

    [Header("Damage by player")]
    public GameObject damage_hitbox;

    [Header("Animation Controller")]
    public RuntimeAnimatorController enemy1_idle_controller;
    public RuntimeAnimatorController enemy1_run_controller;
    public RuntimeAnimatorController enemy1_attack_controller;

    public float distance;
    
    private Animator animator;

    private bool isWatchRight;
    private bool isAttack = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        if(isAttack) return;

        //거리 계산
        distance = Vector3.Distance(transform.position, target.position);

        //플레이어 방향
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);
        }
        else if(direction.x == 0)
        {
            animator.runtimeAnimatorController = enemy1_idle_controller;
        }


        if (distance > stopDistance)
        {
            isAttack = false;

            transform.position += direction * moveSpeed * Time.deltaTime;

            animator.runtimeAnimatorController = enemy1_run_controller;
        }
        else
        {
            Attack();
        }

        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("enemy1 hp: " + hp);
        }
    }

    //공격처리
    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCoolTime)
        {
            isAttack = true;

            animator.runtimeAnimatorController = enemy1_attack_controller;
            lastAttackTime = Time.time;

            Invoke(nameof(EndAttack), attackDuration);
            
            Invoke(nameof(AttackCount), 0.3f);
        }
        else
        {
            animator.runtimeAnimatorController = enemy1_idle_controller;
        }
    }

    void EndAttack()
    {
        isAttack = false;
        animator.runtimeAnimatorController = enemy1_idle_controller;
    }

    //attack 애니메이션 컨트롤러 실행 후 0.3초 후 피격 계산
    void AttackCount()
    {
        player player = target.GetComponent<player>();

        //플레이어와의 거리가 정지거리보다 가까운 경우
        //플레이어 hp--
        distance = Vector3.Distance(transform.position, target.position);

        if(distance < stopDistance)
        {
            player.hp--;
        }
    }

    //플레이어 공격으로인한 데미지 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("attack_hitbox"))
        {
            hp--;

            if(hp <= 0) Dead();
        }

        Debug.Log("enemy1 HP: " + hp);
    }

    //적 처치
    void Dead()
    {
        Destroy(this);
    }
}