using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound; // 🔊 사운드 클립 연결용

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
                player.Heal(1); // 체력 1 회복
            }
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);

            Destroy(gameObject); // 포션 사라짐
        }
    }
}
