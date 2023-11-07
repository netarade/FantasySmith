using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // ȸ�� �ӵ��� ������ ����

    void Update()
    {
        // ���������� ȸ��
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
