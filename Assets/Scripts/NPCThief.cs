using UnityEngine;
using UnityEngine.AI;

public class NPCThief : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject targetMotor; // Ubah dari Transform ke GameObject

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent tidak ditemukan di " + gameObject.name);
            return;
        }
    }

    void Update()
    {
        if (targetMotor != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetMotor.transform.position);
        }
    }

    public void SetTarget(GameObject motor) // Ubah parameter dari Transform ke GameObject
    {
        if (motor == null)
        {
            Debug.LogError("Motor target belum di-assign ke maling.");
            return;
        }

        targetMotor = motor;
        Debug.Log("Maling mengincar motor di posisi: " + targetMotor.transform.position);
    }

    public void StealMotor()
    {
        if (targetMotor != null)
        {
            Debug.Log("Maling mencuri motor!");
            Destroy(targetMotor); // Motor langsung hilang
            Destroy(gameObject, 1f); // Hapus maling setelah aksi
        }
    }
}
