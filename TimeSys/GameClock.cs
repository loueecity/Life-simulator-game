using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTxt = null;
    [SerializeField] private TextMeshProUGUI dateTxt = null;
    [SerializeField] private TextMeshProUGUI seasonTxt = null;
    [SerializeField] private TextMeshProUGUI yearTxt = null;

    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    private void UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // Update time

        gameMinute = gameMinute - (gameMinute % 10);

        string ampm = "";
        string minute;

        if (gameHour >= 12)
        {
            ampm = " pm";
        }
        else
        {
            ampm = " am";
        }

        if (gameHour >= 13)
        {
            gameHour -= 12;
        }

        if (gameMinute < 10)
        {
            minute = "0" + gameMinute.ToString();
        }
        else
        {
            minute = gameMinute.ToString();
        }

        string time = gameHour.ToString() + " : " + minute + ampm;


        timeTxt.SetText(time);
        dateTxt.SetText(gameDayOfWeek + ". " + gameDay.ToString());
        seasonTxt.SetText(gameSeason.ToString());
        yearTxt.SetText("Year " + gameYear);
    }

}
