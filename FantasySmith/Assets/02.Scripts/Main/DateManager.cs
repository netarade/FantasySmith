using UnityEngine;
using UnityEngine.UI;
using System;

public class DateManager : MonoBehaviour
{
    public Text dateText;
    private int currentDay = 1;
    private int currentMonth = 1;
    private int currentYear = 2023;

    private void Start()
    {
        UpdateDateText();
    }

    private void UpdateDateText()
    {
        string date = string.Format("{0:D4}-{1:D2}-{2:D2}", currentYear, currentMonth, currentDay);
        dateText.text = date;
    }

    public void AdvanceDay()
    {
        // ��¥ ������Ʈ ����
        currentDay++;
        if (currentDay > 30) // ����: �� ���� 30�Ϸ� ����
        {
            currentDay = 1;
            currentMonth++;
            if (currentMonth > 12)
            {
                currentMonth = 1;
                currentYear++;
            }
        }
        UpdateDateText();
    }
}
