using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI warningText;

    void Start()
    {
        UpdateMoneyUI(0); // Pastikan uang awal ditampilkan
        warningText.gameObject.SetActive(false); // Sembunyikan warning awal
    }

    public void UpdateMoneyUI(int amount)
    {
        if (moneyText != null)
        {
            moneyText.text = "Uang: " + amount;
        }
    }

    public void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            warningText.text = message;

            // Sembunyikan warning setelah 3 detik
            Invoke("HideWarning", 3f);
        }
    }

    void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}
