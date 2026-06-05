using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 40;

    [Header("Target")]
    public string playerTag = "player";
    public Transform target;

    [Header("Movement")]
    //public float moveSpeed = 1.7f;
    private float stopDistance = 1.5f;

    [Header("Attack")]
    //공격관련수치
    private float attack_near_cooltime = 10f;
    private float attack_range_cooltime = 10f;
    private float attackDuration = 3f;
    public int damage = 1;
    private float last_attack_near_time;
    private float last_attack_range_time;

    [Header("Range Attack")]
    public GameObject enemy3_attack_right_hitbox;
    public GameObject enemy3_attack_left_hitbox;
    public GameObject enemy3_attack_right_warning;
    public GameObject enemy3_attack_left_warning;

    [Header("Damage by player")]
    public GameObject damage_hitbox;

    [Header("Animation Controller")]
    public RuntimeAnimatorController enemy3_idle_controller;
    public RuntimeAnimatorController enemy3_attack_near_controller;
    public RuntimeAnimatorController enemy3_hurt_controller;

    private Animator animator;
    
    [Header("Visual Effect")]
    public GameObject enemy3_shockwave;

    [Header("Sound Effect")]
    public AudioClip enemy3_attack_sound;

    public float distance;

    private bool isWatchRight;
    private bool isAttack = false;
    private bool now_attack_right = true;

    //hurt controller
    private bool isHurt = false;

    private Vector3 position_fixed = new Vector3(0.27f, -2.77f, 0f);

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }

        enemy3_shockwave.SetActive(false);
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);

        //위치 고정
        transform.position = position_fixed;

        if(target.transform.position.x > 0)
        {
            isWatchRight = true;
        }
        else
        {
            isWatchRight = false;
        }

        if (target.transform.position.x > 0)
        {
            transform.localScale = new Vector3(2.3f, 2.3f, 2.3f);
        }
        else if (target.transform.position.x < 0)
        {
            transform.localScale = new Vector3(-2.3f, 2.3f, 2.3f);
        }

        if (distance > stopDistance)
        {
            AttackRange();
        }
        else
        {
            AttackNear();
        }
    }

    void AttackNear()
    {
        if (Time.time - last_attack_near_time >= attack_near_cooltime)
        {
            isAttack = true;

            animator.runtimeAnimatorController = enemy3_attack_near_controller;
            last_attack_near_time = Time.time;

            enemy3_shockwave.SetActive(true);

            Invoke(nameof(EndAttack), attackDuration);

            AudioSource.PlayClipAtPoint(enemy3_attack_sound, transform.position);
            
            Invoke(nameof(AttackCount), 1f);
        }
        else
        {
            animator.runtimeAnimatorController = enemy3_idle_controller;
        }
    }

    void EndAttack()
    {
        isAttack = false;
        animator.runtimeAnimatorController = enemy3_idle_controller;
        enemy3_shockwave.SetActive(false);
    }

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

    //player가 stopdistance 밖에 있을 때
    void AttackRange()
    {
        if (Time.time - last_attack_range_time < attack_range_cooltime) return;

        last_attack_range_time = Time.time;

        if (now_attack_right == true)
        {
            GameObject attack_right_warning_prefab = Instantiate(enemy3_attack_right_warning);

            Invoke(nameof(SpawnRightAttack), 3f);

            Destroy(attack_right_warning_prefab, 3f);
        }
        else
        {
            GameObject attack_left_warning_prefab = Instantiate(enemy3_attack_left_warning);

            Invoke(nameof(SpawnLeftAttack), 3f);

            Destroy(attack_left_warning_prefab, 3f);
        }
        now_attack_right = false;
    }

    void SpawnRightAttack()
    {
        GameObject attack_right_hitbox_prefab = Instantiate(enemy3_attack_right_hitbox);

        Destroy(attack_right_hitbox_prefab, 3f);
    }
    void SpawnLeftAttack()
    {
        GameObject attack_left_hitbox_prefab = Instantiate(enemy3_attack_left_hitbox);

        Destroy(attack_left_hitbox_prefab, 3f);
    }

    //플레이어 공격으로인한 데미지 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("attack_hitbox"))
        {
            animator.runtimeAnimatorController = enemy3_hurt_controller;
            Invoke(nameof(HurtAnimation), 0.2f);

            hp--;
        }
    }
    void HurtAnimation()
    {
        Debug.Log("Enemy3: " + hp);
    }
}
