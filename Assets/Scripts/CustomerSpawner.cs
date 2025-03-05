using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; } 

    public GameObject customerPrefab;
    public GameObject motorPrefab;
    public Transform[] spawnPoints; 
    public Transform exitPoint;
    public float spawnInterval = 5f;
    public List<Transform> parkingSlots = new List<Transform>();
    public ThiefSpawner thiefSpawner; 
    private List<Transform> availableSlots = new List<Transform>();
   

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

                // Pilih titik spawn secara acak
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                NPCCustomer npc = newCustomer.GetComponent<NPCCustomer>();

                GameObject motor = Instantiate(motorPrefab, spawnPoint.position + new Vector3(0, -0.05f, 0), Quaternion.identity);

                //  Pastikan fungsi ini menggunakan parameter yang benar
                npc.AssignParkingSlot(selectedSlot, this, motor, thiefSpawner);
            }
            else
            {
                Debug.Log("Semua slot parkir penuh!");
            }
        }
    }

    //Menandai parkiran sebagai kosong saat maling mencuri motor atau NPC pergi
    public void FreeSlot(Transform slot)
    {
        if (!availableSlots.Contains(slot))
        {
            availableSlots.Add(slot);
        }
    }

   

    // Untuk Cek apakah parkiran kosong
    public bool IsSlotAvailable(Transform slot)
    {
        return availableSlots.Contains(slot);
    }

    // Ambil slot parkir yang kosong
    public Transform GetAvailableSlot()
    {
        if (availableSlots.Count > 0)
        {
            return availableSlots[0];
        }
        return null;
    }

    public Transform GetExitPoint()
    {
        return exitPoint;
    }
}
