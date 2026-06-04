using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 40;

    [Header("Target")]
    public string playerTag = "player";
    public Transform target;

    [Header("Movement")]
    public float moveSpeed = 1.7f;
    private float stopDistance = 1.3f;

    [Header("Attack")]
    //공격관련수치
    public float attack_near_cooltime = 10f;
    private float attackDuration = 3f;
    public int damage = 1;
    private float last_attack_near_time;

    [Header("Damage by player")]
    public GameObject damage_hitbox;

    [Header("Animation Controller")]
    public RuntimeAnimatorController enemy3_idle_controller;
    public RuntimeAnimatorController enemy3_attack_near_controller;
    //public RuntimeAnimatorController enemy3_hurt_controller;

    private Animator animator;
    
    [Header("Visual Effect")]
    public GameObject enemy3_shockwave;

    [Header("Sound Effect")]
    public AudioClip enemy3_attack_sound;

    public float distance;

    private bool isWatchRight;
    private bool isAttack = false;

    //hurt controller
    private bool isHurt = false;

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
            isAttack = false;

            //transform.position += direction * moveSpeed * Time.deltaTime;

            //animator.runtimeAnimatorController = enemy3_run_controller;
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
            
            Invoke(nameof(AttackCount), 0.3f);
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
}
