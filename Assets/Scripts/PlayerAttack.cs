using UnityEngine;
using StarterAssets; // Pastikan ini di-import

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask thiefLayer;
    public int attackDamage = 10;
    private StarterAssetsInputs input; // Reference ke sistem input StarterAssets

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>(); // Ambil komponen StarterAssetsInputs
    }

    void Update()
    {
        if (input == null)
        {
            Debug.LogError("StarterAssetsInputs tidak ditemukan di Player!");
            return;
        }

        if (input.attack) // Penyesuaian dengan action attack di Input System
        {
            Attack();
            input.attack = false; // Reset attack setelah serangan dilakukan
        }
    }

    void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint belum di-assign!");
            return;
        }

        Debug.Log("Serangan dilakukan!");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, thiefLayer);
        Debug.Log("Musuh terkena: " + hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Mendeteksi collider: " + enemy.name);

            NPCThief thief = enemy.GetComponent<NPCThief>();
            if (thief != null)
            {
                Debug.Log("Maling terkena serangan!");
                thief.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("Collider terdeteksi, tapi tidak memiliki script NPCThief!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
