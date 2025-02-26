using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCCustomer : MonoBehaviour
{
    public Transform parkingSlot;
    public GameObject motorPrefab;
    public ThiefSpawner thiefSpawner; // ✅ Pastikan ini digunakan dengan benar

    private NavMeshAgent agent;
    private bool hasPlacedMotor = false;
    private bool hasLeft = false;
    private CustomerSpawner spawner;
    private GameObject motorInstance;

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

        // Pastikan motor tidak null sebelum maling spawn
        if (thiefSpawner != null && motorInstance != null)
        {
            thiefSpawner.SpawnThief(parkingSlot, motorInstance);
        }

        StartCoroutine(DecideToLeave());
    }
}

    public void AssignParkingSlot(Transform slot, CustomerSpawner spawnerRef, GameObject motor, ThiefSpawner thiefSpawnerRef)
    {
        parkingSlot = slot;
        spawner = spawnerRef;
        motorInstance = motor;
        motorInstance.transform.SetParent(this.transform); // Motor ikut NPC sampai parkir
        thiefSpawner = thiefSpawnerRef; // ✅ Pastikan ini benar
    }

    void PlaceMotor()
    {
        if (motorInstance != null)
        {
            motorInstance.transform.SetParent(null); // ✅ Pastikan motor dilepas dari NPC
            motorInstance.transform.position = parkingSlot.position + new Vector3(0, 0.1f, 0);
            motorInstance.transform.rotation = Quaternion.LookRotation(parkingSlot.forward);
            Debug.Log($"Motor diparkir di {motorInstance.transform.position}");
        }
        else
        {
            Debug.LogError("Motor belum diassign!");
        }
    }

    IEnumerator DecideToLeave()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6f));

        if (!hasLeft)
        {
            hasLeft = true;
            bool willPay = Random.value > 0.2f; // 80% bayar, 20% kabur

            if (willPay)
            {
                PayAndLeave();
            }
            else
            {
                RunAway();
            }
        }
    }

    void PayAndLeave()
    {
        Debug.Log("NPC membayar dan pergi.");
        if (spawner != null)
        {
            spawner.FreeSlot(parkingSlot);
        }
        LeaveParking();
    }

    void RunAway()
    {
        Debug.Log("NPC kabur tanpa bayar!");
        LeaveParking();
    }

    void LeaveParking()
    {
        if (spawner != null)
        {
            Transform exitPoint = spawner.GetExitPoint();
            if (exitPoint != null)
            {
                agent.SetDestination(exitPoint.position);
                Debug.Log("NPC menuju exit point.");

                // ✅ Pastikan motor tetap di parkiran
                if (motorInstance != null)
                {
                    motorInstance.transform.SetParent(null);
                }
            }
            else
            {
                Debug.LogError("Exit point tidak ditemukan!");
            }
        }
        else
        {
            Debug.LogError("Spawner tidak ditemukan!");
        }

        Destroy(gameObject, 5f);
    }
}
