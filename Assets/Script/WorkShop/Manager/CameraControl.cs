using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 6, -5);
    public float rotationAngle = 45f;   // เพิ่มเข้ามา

    void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // หมุนกล้องให้มองลงมา
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }

}
