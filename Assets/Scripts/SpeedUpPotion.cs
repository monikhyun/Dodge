using UnityEngine;

public class SpeedUpPotion : MonoBehaviour
{
    [SerializeField]private AudioClip collectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateSpeedUp();
            }
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);

            Destroy(gameObject); // 포션 사라짐
        }
    }
}