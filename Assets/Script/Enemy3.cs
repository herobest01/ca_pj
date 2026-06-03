using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 15;

    [Header("Target")]
    public string playerTag = "player";
    public Transform target;

    [Header("Attack")]
    //공격관련수치
    public float attackCoolTime = 1f;
    public int damage = 1;
    private float lastAttackTime;
    private float attackDuration = 1f;

    [Header("Damage by player")]
    public GameObject damage_hitbox;

    //[Header("Animation Controller")]
    //public RuntimeAnimatorController enemy3_idle_controller;
    //public RuntimeAnimatorController enemy3_attack_controller;
    //public RuntimeAnimatorController enemy3_hurt_controller;

    private Animator animator;

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
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);
    }
}
