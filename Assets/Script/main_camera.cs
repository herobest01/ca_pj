using UnityEngine;

public class main_camera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 1.75f, -10f);
    public Vector3 origin_position;
    
    public Vector3 playerAttacked_position;

    public bool isAttacked;
    private bool camera_control = false;

    void Start()
    {
        Camera.main.orthographicSize = 2.5f;
    }

    void Update()
    {
        if(camera_control == true) return;

        playerAttacked_position = new Vector3(transform.position.x, -3f, transform.position.z);
        origin_position = new Vector3(transform.position.x, -1.5f, -10f);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void PlayerAttacked()
    {
        camera_control = true;
        Camera.main.transform.position = playerAttacked_position;
        Camera.main.orthographicSize = 1.5f;
        Invoke(nameof(CameraSizeChange), 1f);
    }

    void CameraSizeChange()
    {
        //기본값으로 복구
        camera_control = false;
        Camera.main.transform.position = origin_position;
        Camera.main.orthographicSize = 2.5f;
    }
}
