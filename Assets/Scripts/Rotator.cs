using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float changeDirectionInterval = 10f; // 방향 바꾸는 주기 (초)

    private float timer = 0f;
    private int direction = 1; // 1 또는 -1

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= changeDirectionInterval)
        {
            direction *= -1; // 방향 반전
            timer = 0f; // 타이머 초기화
        }

        transform.Rotate(0f, direction * rotationSpeed * Time.fixedDeltaTime, 0f);
    }
}