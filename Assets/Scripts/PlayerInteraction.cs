using StarterAssets;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private StarterAssetsInputs input;
    private NPCCustomer targetedNPC;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (input.interact && targetedNPC != null)
        {
            targetedNPC.StopAndPay(); // NPC membayar saat ditekan
            input.interact = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && other.TryGetComponent(out NPCCustomer npc))
        {
            // Cek apakah NPC ini kabur
            if (npc.isRunningAway)
            {
                targetedNPC = npc;
                UIInteractManager.Instance.ShowInteractMessage("Tekan [F] untuk Menghadang!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            targetedNPC = null;
            UIInteractManager.Instance.HideInteractMessage();
        }
    }
}
