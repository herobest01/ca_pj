using UnityEngine;

public class htibox : MonoBehaviour
{
    public Transform target;
    public Vector3 hitbox_pos = new Vector3(0f, 0f, 0f);

    public GameObject hitbox;

    public bool spawn_hitbox = false;

    void Start()
    {
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn_hitbox == false) return;

        if(spawn_hitbox == true)
        {
            hitbox_pos = new Vector3(target.position.x, target.position.y, 0f);
            hitbox.SetActive(true);
        }
    }

    //히트박스 객체 생성 여부
    public void SpawnHitbox()
    {
        spawn_hitbox = true;
    }
}
