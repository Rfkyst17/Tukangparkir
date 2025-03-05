using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class NPCCustomer : MonoBehaviour
{
    public Transform parkingSlot;
    public GameObject motorPrefab;
    public ThiefSpawner thiefSpawner;

    private NavMeshAgent agent;
    private bool hasPlacedMotor = false;
    private bool hasLeft = false;
    private CustomerSpawner spawner;
    private GameObject motorInstance;
    private bool kaburTanpaBayar = false; 
    public bool hasPaid = false;
    public bool isRunningAway = false;

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

            // Jika maling ada, spawn setelah motor diparkir
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
        motorInstance.transform.SetParent(this.transform);
        thiefSpawner = thiefSpawnerRef;
    }

    void PlaceMotor()
    {
        if (motorInstance != null)
        {
            motorInstance.transform.SetParent(null);
            motorInstance.transform.position = parkingSlot.position + new Vector3(0, -0.05f, 0);
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
            kaburTanpaBayar = Random.value < 0.3f; // 20% kemungkinan kabur

            if (kaburTanpaBayar)
            {
                RunAway();
            }
            else
            {
                PayAndLeave();
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
        GameManager.Instance.AddMoney(5000); // Tambahkan uang saat NPC membayar
        LeaveParking();
    }

    void RunAway()
    {
        Debug.Log("NPC kabur tanpa bayar!");
        GameManager.Instance.NPCKabur(); // Panggil fungsi untuk memberi warning di UI

        isRunningAway = true; // Tandai NPC ini sedang kabur
        LeaveParking();
    }

    public void StopAndPay()
    {
        if (!hasPaid)
        {
            hasPaid = true;
            GameManager.Instance.AddMoney(5000); // NPC akhirnya bayar!
            Debug.Log("NPC membayar dan pergi dengan tenang.");

            // Hapus UI interaksi
            GameManager.Instance.HideInteractMessage();

            // NPC pergi dengan normal
            LeaveParking();
        }
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

                //  motor tetap di parkiran
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
