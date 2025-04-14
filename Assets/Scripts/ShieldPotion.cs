using UnityEngine;

public class ShieldPotion : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateInvincibility();
            }
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
            Destroy(gameObject); // 포션 제거
        }
    }
}
