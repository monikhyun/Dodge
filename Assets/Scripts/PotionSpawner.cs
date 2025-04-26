using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] potionPrefabs;
    [SerializeField] private float spawntime = 2f;
    [SerializeField] private float collisionCheckRadius = 1f;
    [SerializeField] private LayerMask collisionMask;

    private Vector3 center;
    private Vector3 size;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        center = transform.position;
        size = new Vector3(transform.localScale.x * 10f, 0f, transform.localScale.z * 10f); // Unity Plane은 10x10

        InvokeRepeating(nameof(SpawnPotion), 1f, spawntime);
    }

    void SpawnPotion()
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector3 pos = GetRandomPositionOnPlane();

            bool overlaps = Physics.CheckSphere(pos, collisionCheckRadius, collisionMask);
            if (!overlaps)
            {
                GameObject prefab = potionPrefabs[Random.Range(0, potionPrefabs.Length)];
                Instantiate(prefab, pos, Quaternion.identity);
                return;
            }
        }
    }
    Vector3 GetRandomPositionOnPlane()
    {
        float spawnRangeX = size.x * 0.7f; 
        float spawnRangeZ = size.z * 0.7f;

        float x = Random.Range(-spawnRangeX / 2f, spawnRangeX / 2f);
        float z = Random.Range(-spawnRangeZ / 2f, spawnRangeZ / 2f);
        float y = -0.5f;
        return new Vector3(center.x + x, y, center.z + z);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
