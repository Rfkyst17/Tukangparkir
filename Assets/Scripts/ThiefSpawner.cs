using UnityEngine;

public class ThiefSpawner : MonoBehaviour
{
    public GameObject thiefPrefab;
    public Transform[] thiefSpawnPoints; 
    public Transform[] thiefExitPoints;   

    public void SpawnThief(Transform parkingSlot, GameObject motor)
    {
        if (thiefPrefab == null || motor == null || parkingSlot == null)
        {
            Debug.LogError("ThiefSpawner: Salah satu parameter tidak valid!");
            return;
        }

        // ✅ Pilih titik spawn maling secara acak
        Transform spawnPoint = thiefSpawnPoints.Length > 0
            ? thiefSpawnPoints[Random.Range(0, thiefSpawnPoints.Length)]
            : null;

        // ✅ Pilih titik exit maling secara acak
        Transform exitPoint = thiefExitPoints.Length > 0
            ? thiefExitPoints[Random.Range(0, thiefExitPoints.Length)]
            : null;

        if (spawnPoint == null || exitPoint == null)
        {
            Debug.LogError("ThiefSpawner: Tidak ada spawn point atau exit point yang tersedia!");
            return;
        }

        // Jika tidak ada spawn point yang tersedia, spawn maling dekat parkiran
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : parkingSlot.position + new Vector3(1f, 0, 1f);
        GameObject thiefInstance = Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
        NPCThief thiefScript = thiefInstance.GetComponent<NPCThief>();

        if (thiefScript != null)
        {
            //  Berikan target motor, exit point, dan slot parkir ke maling
            thiefScript.SetTarget(motor, exitPoint, parkingSlot);
        }
        else
        {
            Debug.LogError("Script NPCThief tidak ditemukan di maling.");
        }
    }
}
