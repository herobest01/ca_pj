using UnityEngine;

public class main_camera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 1.75f, -10f);

    public bool isAttacked;

    void Start()
    {
        Camera.main.orthographicSize = 2.5f;
    }

    void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void PlayerAttacked()
    {
        Camera.main.orthographicSize = 1.5f;
        Invoke(nameof(CameraSizeChange), 1f);
    }

    void CameraSizeChange()
    {
        Camera.main.orthographicSize = 2.5f;
    }
}
