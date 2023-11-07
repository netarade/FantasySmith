using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text todayGoldText;
    public Text todaySilverText;
    public Text totalGoldText;
    public Text totalSilverText;

    private int todayGold = 0; // ���� �� ��ȭ
    private int todaySilver = 0; // ���� �� ��ȭ
    private int totalGold = 0; // ���� ��ȭ
    private int totalSilver = 0; // ���� ��ȭ

    private void Start()
    {
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        todayGoldText.text = "" + todayGold;
        todaySilverText.text = "" + todaySilver;
        totalGoldText.text = "" + totalGold;
        totalSilverText.text = "" + totalSilver;
    }

    public void EarnTodayGold(int amount)
    {
        todayGold += amount;
        UpdateMoneyText();
    }

    public void EarnTodaySilver(int amount)
    {
        todaySilver += amount;
        UpdateMoneyText();
    }

    public void ConvertTodayToTotal()
    {
        totalGold += todayGold;
        totalSilver += todaySilver;
        todayGold = 0;
        todaySilver = 0;
        UpdateMoneyText();
    }
}
