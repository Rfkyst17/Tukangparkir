using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject motorPrefab; // Tambahan: Prefab motor
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public List<Transform> parkingSlots = new List<Transform>(); // Semua slot parkir
    private List<Transform> availableSlots = new List<Transform>(); // Slot yang masih kosong

    void Start()
    {
        availableSlots = new List<Transform>(parkingSlots); // Copy daftar slot awal
        StartCoroutine(SpawnCustomer());
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (availableSlots.Count > 0) // Hanya spawn jika ada slot kosong
            {
                Transform selectedSlot = availableSlots[Random.Range(0, availableSlots.Count)];
                availableSlots.Remove(selectedSlot); // Hapus dari daftar slot yang kosong

                GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                NPCCustomer npc = newCustomer.GetComponent<NPCCustomer>();

                // Spawn motor bersamaan dengan NPC
                GameObject motor = Instantiate(motorPrefab, spawnPoint.position, Quaternion.identity);
                npc.AssignParkingSlot(selectedSlot, this, motor); // Kirim slot dan motor ke NPC
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
            availableSlots.Add(slot); // Slot tersedia lagi setelah NPC pergi
        }
    }
}



