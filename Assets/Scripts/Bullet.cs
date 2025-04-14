using System;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float splashRadius = 5f;
    private Rigidbody bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.isKinematic = false;

        Vector3 launchDirection = transform.forward + Vector3.up * 0.5f;
        bulletRigidbody.linearVelocity = launchDirection.normalized * speed;

        Invoke(nameof(Explode), 2.2f);
    }

    private void Explode()
    {

        // 폭발 범위 내 플레이어 감지 및 데미지 적용
        Collider[] hits = Physics.OverlapSphere(transform.position, splashRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage();
                }
            }
        }

        // 0.2초 후 파괴
        Invoke(nameof(DestroyBomb), 0.2f);
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die();
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
        }
    }
}