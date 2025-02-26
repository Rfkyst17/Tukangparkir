using UnityEngine;

public class ThiefSpawner : MonoBehaviour
{
    public GameObject thiefPrefab;
    public Transform thiefSpawnPoint; // Titik spawn maling

    public void SpawnThief(Transform parkingSlot, GameObject motor)
    {
        if (thiefPrefab == null)
        {
            Debug.LogError("Prefab maling belum di-assign di ThiefSpawner.");
            return;
        }

        if (motor == null)
        {
            Debug.LogError("Motor untuk maling belum tersedia.");
            return;
        }

        // Pastikan titik spawn maling sudah di-assign
        Vector3 spawnPosition = (thiefSpawnPoint != null) ? thiefSpawnPoint.position : parkingSlot.position + new Vector3(1f, 0, 1f);

        GameObject thiefInstance = Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
        NPCThief thiefScript = thiefInstance.GetComponent<NPCThief>();

        if (thiefScript != null)
        {
            thiefScript.SetTarget(motor);
        }
        else
        {
            Debug.LogError("Script NPCThief tidak ditemukan di maling.");
        }
    }
}
