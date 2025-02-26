using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject motorPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public float spawnInterval = 5f;
    public List<Transform> parkingSlots = new List<Transform>();
    public ThiefSpawner thiefSpawner; // ✅ Tambahkan reference ke ThiefSpawner
    private List<Transform> availableSlots = new List<Transform>();

    void Start()
    {
        availableSlots = new List<Transform>(parkingSlots);
        StartCoroutine(SpawnCustomer());
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (availableSlots.Count > 0)
            {
                Transform selectedSlot = availableSlots[Random.Range(0, availableSlots.Count)];
                availableSlots.Remove(selectedSlot);

                GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                NPCCustomer npc = newCustomer.GetComponent<NPCCustomer>();

                GameObject motor = Instantiate(motorPrefab, spawnPoint.position, Quaternion.identity);

                // ✅ Pastikan fungsi ini menggunakan parameter yang benar
                npc.AssignParkingSlot(selectedSlot, this, motor, thiefSpawner);
            }
            else
            {
                Debug.Log("Semua slot parkir penuh!");
            }
        }
    }

    public void FreeSlot(Transform slot)
    {
        if (!availableSlots.Contains(slot))
        {
            availableSlots.Add(slot);
        }
    }

    public Transform GetExitPoint()
    {
        return exitPoint;
    }
}
