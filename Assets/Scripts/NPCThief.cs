using UnityEngine;
using UnityEngine.AI;

public class NPCThief : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform targetMotor;
    private Transform exitPoint;
    private Transform parkingSlot; // ✅ Referensi ke parkiran
    private bool hasStolen = false;
    public int health = 2; // Nyawa maling
    public float knockbackForce = 1.5f;

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
        if (targetMotor != null && !hasStolen && agent.isOnNavMesh)
        {
            agent.SetDestination(targetMotor.position);

            // Jika maling sudah sampai di motor, curi motor
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StealMotor();
            }
        }

        if (hasStolen && exitPoint != null)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                GameManager.Instance.SubtractMoney(2500);
                Destroy(targetMotor.gameObject); // Hapus motor
                Destroy(gameObject); // Hapus maling setelah sampai di exit
                Debug.Log("Maling berhasil kabur dengan motor! -Rp 2.500");
            }
        }
    }

    public void SetTarget(GameObject motor, Transform exit, Transform slot)
    {
        if (motor == null || exit == null || slot == null)
        {
            Debug.LogError("Motor, Exit Point, atau Slot Parkir belum di-assign ke maling.");
            return;
        }

        targetMotor = motor.transform;
        exitPoint = exit;
        parkingSlot = slot; // ✅ Simpan referensi parkiran
        Debug.Log("Maling mengincar motor & akan kabur ke: " + exitPoint.position);

        // ✅ Tandai slot parkir sebagai kosong saat maling mulai datang
        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.FreeSlot(parkingSlot);
        }
    }

    void StealMotor()
    {
        if (hasStolen) return;

        hasStolen = true;
        Debug.Log("Maling mencuri motor!");

        //  motor sebagai anak dari maling biar ikut bergerak
        targetMotor.SetParent(this.transform);
        targetMotor.localPosition = new Vector3(0, -0.05f, 0f);
        targetMotor.rotation = transform.rotation;

        //  Mematikan collider dan physics motor agar tidak bertabrakan
        if (targetMotor.GetComponent<Collider>() != null)
        {
            targetMotor.GetComponent<Collider>().enabled = false;
        }

        if (targetMotor.GetComponent<Rigidbody>() != null)
        {
            targetMotor.GetComponent<Rigidbody>().isKinematic = true;
        }

        //  Perintah maling kabur ke exit point
        if (exitPoint != null)
        {
            agent.SetDestination(exitPoint.position);
        }
    }

    // Maling bisa dipukul oleh player
    public void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " terkena serangan! HP - " + damage);
        health -= damage;

        if (health > 0)
        {
            KnockBack();
        }
        else
        {
            Die();
        }
    }

    // Efek Knockback
    void KnockBack()
    {
        if (agent.isOnNavMesh)
        {
            Vector3 knockbackDirection = transform.position - Camera.main.transform.position;
            Vector3 newPosition = transform.position + knockbackDirection.normalized * knockbackForce;

            // Pastikan maling tidak keluar dari NavMesh
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    // 🔹**Efek Maling Mati**
    void Die()
    {
        Debug.Log("Maling tumbang!");
        agent.isStopped = true;

        // Jika maling belum mencuri, hapus mobilnya
        if (!hasStolen && targetMotor != null)
        {
            Debug.Log("Maling gagal! Mobil dihapus dari parkiran.");
            Destroy(targetMotor.gameObject);  // Hapus mobilnya
        }

        // Kosongkan slot parkir agar mobil baru bisa parkir
        if (parkingSlot != null)
        {
            CustomerSpawner.Instance.FreeSlot(parkingSlot);
        }

        Destroy(gameObject, 2f);
    }


}
