using UnityEngine;

public class hitbox : MonoBehaviour
{
    public player target;
    public Vector3 hitbox_pos = new Vector3(0f, 0f, 0f);

    public GameObject hitbox_obj;

    public bool spawn_hitbox = false;

    //공격시 발생하는 효과음관련 코드는 player.cs에

    void Start()
    {
        hitbox_obj.SetActive(false);
    }

    void Update()
    {
        target = GetComponent<player>();

        if(spawn_hitbox == false) return;

        if(spawn_hitbox == true)
        {
            hitbox_pos = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
            hitbox_obj.SetActive(true);
        }
    }

    //히트박스 객체 생성 여부
    public void SpawnHitbox()
    {
        spawn_hitbox = true;
    }
}
