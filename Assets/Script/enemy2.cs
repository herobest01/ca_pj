using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 1;

    [Header("Target")]
    public string playerTag = "Player";
    private Transform target;

    [Header("Movement")]
    public float moveSpeed = 1.7f;
    public float stopDistance = 1.3f;

    [Header("Attack")]
    //공격관련수치
    public float attackCoolTime = 5f;
    public int damage = 1;
    private float lastAttackTime;
    private float attackDuration = 1f;

    [Header("Damage by player")]
    public GameObject damage_hitbox;

    [Header("Enemy2 Aniamtion")]
    public RuntimeAnimatorController enemy2_idle_controller;
    public RuntimeAnimatorController enemy2_run_controller;
    public RuntimeAnimatorController enemy2_attack_controller;
    public RuntimeAnimatorController enemy2_hurt_controller;

    [Header("Sound Effect")]
    public AudioClip enemy2_attack_sound;

    //15마리 소환관련
    [Header("Spawn")]
    public GameObject enemy2_prefab;
    public int spawn_max = 15;
    public int spawn_count = 0;
    public static int death_count = 0;

    private Animator animator;

    public bool isAttack = false;

    public float distance;

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

        if(hp <= 0) return;

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
            animator.runtimeAnimatorController = enemy2_idle_controller;
        }


        if (distance > stopDistance)
        {
            isAttack = false;

            transform.position += direction * moveSpeed * Time.deltaTime;

            animator.runtimeAnimatorController = enemy2_run_controller;
        }
        else
        {
            Attack();
        }
    }

    public void Summon()
    {
        for(int spawn_count=0; spawn_count < spawn_max; spawn_count++)
        {
            Invoke(nameof(SummonStart), 2.5f * spawn_count);
        }
    }
    void SummonStart()
    {
        Vector3 spawnPos = new Vector3(-7.7f, -3f, 0f);
        Instantiate(enemy2_prefab, spawnPos, Quaternion.identity);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCoolTime)
        {
            isAttack = true;

            animator.runtimeAnimatorController = enemy2_attack_controller;
            lastAttackTime = Time.time;

            Invoke(nameof(EndAttack), attackDuration);

            AudioSource.PlayClipAtPoint(enemy2_attack_sound, transform.position);
            
            Invoke(nameof(AttackCount), 0.3f);
        }
        else
        {
            animator.runtimeAnimatorController = enemy2_idle_controller;
        }
    }

    void EndAttack()
    {
        isAttack = false;
        animator.runtimeAnimatorController = enemy2_idle_controller;
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
            animator.runtimeAnimatorController = enemy2_hurt_controller;
            Invoke(nameof(HurtAnimation), 0.21f);

            hp--;
            death_count++;
        }
    }

    void HurtAnimation()
    {
        Destroy(gameObject);
        
        Debug.Log("deathcount: " + death_count);
    }
}
