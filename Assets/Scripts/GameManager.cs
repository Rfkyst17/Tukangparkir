using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int money = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI warningText; 

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

    private void Start()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public void NPCKabur()
    {
        if (warningText != null)
        {
            warningText.text = "NPC Kabur Tanpa Bayar!";
            StartCoroutine(ClearWarning());
        }
    }

    public void SubtractMoney(int amount)
    {
        money -= amount;
        UpdateMoneyUI();
    }

    private IEnumerator ClearWarning()
    {
        yield return new WaitForSeconds(3f);
        if (warningText != null)
        {
            warningText.text = "";
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Uang: Rp " + money;
        }
    }

    public void ShowInteractMessage(string message)
    {
        Debug.Log("Pesan Interaksi: " + message);
        
    }

    public void HideInteractMessage()
    {
        Debug.Log("Pesan Interaksi disembunyikan");
       
    }
}
