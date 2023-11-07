using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text todayGoldText;
    public Text todaySilverText;
    public Text totalGoldText;
    public Text totalSilverText;

    private int todayGold = 0; // 오늘 번 금화
    private int todaySilver = 0; // 오늘 번 은화
    private int totalGold = 0; // 보유 금화
    private int totalSilver = 0; // 보유 은화

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
