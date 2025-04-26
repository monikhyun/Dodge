using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Animator animator;
    public float animSpeedMin = 0.8f;
    public float animSpeedMax = 1.2f;

    private Transform target;
    private bool isAttacking = false;
    private float animDuration = 2.167f;
    private float timeSinceAttack = 0f;
    private bool hasThrown = false; // New field

    void Start()
    {
        PlayerAutoMove autoMove = FindFirstObjectByType<PlayerAutoMove>();
        if (autoMove != null)
        {
            target = autoMove.transform;
        }
        else
        {
            PlayerController controller = FindFirstObjectByType<PlayerController>();
            if (controller != null)
            {
                target = controller.transform;
            }
            else
            {
                Debug.LogError("PlayerAutoMove나 PlayerController가 씬에 없습니다!");
            }
        }
    }

    void Update()
    {
        if (!isAttacking)
        {
            float animSpeed = Random.Range(animSpeedMin, animSpeedMax);
            animator.speed = animSpeed;
            animator.SetTrigger("Attack");

            isAttacking = true;
            timeSinceAttack = 0f;
            hasThrown = false; // reset throw flag
        }
        else
        {
            timeSinceAttack += Time.deltaTime;

            if (timeSinceAttack >= animDuration / animator.speed)
            {
                isAttacking = false;
            }
        }
    }

    public void ThrowBomb()
    {
        if (hasThrown) return; // prevent multiple throws
        hasThrown = true;

        Vector3 spawnOffset = transform.up * 1.5f + transform.right * 0.5f;
        GameObject bullet = Instantiate(bulletPrefab, transform.position + spawnOffset, transform.rotation);
        bullet.transform.LookAt(target);
    }
}