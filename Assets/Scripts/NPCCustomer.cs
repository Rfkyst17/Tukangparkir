using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCCustomer : MonoBehaviour
{
    public Transform parkingSlot;
    private NavMeshAgent agent;
    private bool hasPlacedMotor = false;
    private CustomerSpawner spawner;
    private GameObject motor; // Motor yang ikut NPC

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (parkingSlot != null)
        {
            agent.SetDestination(parkingSlot.position);
        }
        else
        {
            Debug.LogError("Parking slot tidak ditetapkan.");
        }
    }

    void Update()
    {
        if (!hasPlacedMotor && agent.remainingDistance <= agent.stoppingDistance)
        {
            PlaceMotor();
            hasPlacedMotor = true;
            StartCoroutine(LeaveParking());
        }
    }

    public void AssignParkingSlot(Transform slot, CustomerSpawner spawnerRef, GameObject assignedMotor)
    {
        parkingSlot = slot;
        spawner = spawnerRef;
        motor = assignedMotor;

        // Pastikan motor mengikuti NPC sampai sampai di parkiran
        motor.transform.SetParent(transform);
    }


    void PlaceMotor()
    {
        if (motor != null)
        {
            // Pastikan motor diletakkan di parkiran dengan rotasi yang benar
            motor.transform.position = parkingSlot.position + new Vector3(0, 0.1f, 0);
            motor.transform.rotation = parkingSlot.rotation;
        }
        else
        {
            Debug.LogError("Motor tidak ditemukan!");
        }
    }

    IEnumerator LeaveParking()
    {
        yield return new WaitForSeconds(3f); // Tunggu sebelum pergi

        if (spawner != null)
        {
            spawner.FreeSlot(parkingSlot); // Kosongkan slot parkir sebelum pergi
        }

        Destroy(gameObject); // NPC dihapus dari scene
    }
}



