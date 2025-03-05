using UnityEngine;
using TMPro;

public class UIInteractManager : MonoBehaviour
{
    public static UIInteractManager Instance;
    public TextMeshProUGUI interactText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideInteractMessage(); // Sembunyikan teks di awal
    }

    public void ShowInteractMessage(string message)
    {
        interactText.text = message;
        interactText.alpha = 1; // Tampilkan teks
    }

    public void HideInteractMessage()
    {
        interactText.alpha = 0; // Sembunyikan teks
    }
}
