using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManage : SingletonMonoBehaviour<TimeManage>
{
  private int gameYear = 1;
  private Season gameSeason  = Season.Spring;
  private int gameDay =1;
  private int gameHour = 6;
  private int gameMinute = 30;
  private int gameSecond = 0;
  private string gameDayWeek = "Monday";
  private bool clockPaused = false;
  private float gameTick = 0f;

  private void Start()
  {
      EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }


private void Update(){
    if(!clockPaused){
        GameTick();
    }
}
  private void GameTick()
  {
      gameTick += Time.deltaTime;
      if(!clockPaused){
        if(gameTick >= Settings.secondsPerGameSecond){
            gameTick -= Settings.secondsPerGameSecond;
            UpdateGameSecond();
        }
      }
  }

    public void TestAdvanceGameDay()
    {
        for (int i = 0; i < 86400; i++)
        {
            UpdateGameSecond();
        }
    }

    private void UpdateGameSecond(){
      gameSecond++;

      if(gameSecond>59){
          gameSecond=0;
          gameMinute++;

          if(gameMinute>59){
              gameMinute =0;
              gameHour++;

              if(gameHour>23){
                  gameHour=0;
                  gameDay++;
                  
                  if(gameDay>30){
                      gameDay = 1;
                      int gseason =(int)gameSeason;
                      gseason++;
                      if(gseason >3){
                          gseason=0;
                          gameSeason=(Season)gseason;
                          gameYear++;
                          EventHandler.CallAdvanceGameYearEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
                      }
                      EventHandler.CallAdvanceGameSeasonEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
                  }
                  gameDayWeek = GetDayOfWeek();
                    Character.instance.energy.setNumber(1000);
                    Character.instance.updateBar();
                  EventHandler.CallAdvanceGameDayEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
              }
              EventHandler.CallAdvanceGameHourEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
          }
          EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
          Debug.Log("gameyear:" + gameYear + " game season: " + gameSeason + " game day: " + gameDay + " game hour: "+ gameHour +" game minute: " + gameMinute + " game second: " + gameSecond);
      }
  }

private string GetDayOfWeek(){
    int totalDays = (((int)gameSeason) *30)+gameDay;
    int dayOfWeek = totalDays % 7;

    switch(dayOfWeek)
    {
        case 1:
        return "Monday";
        case 2:
        return "Tuesday";
        case 3:
        return "Wednesday";
        case 4:
        return "Thursday";
        case 5:
        return "Friday";
        case 6: 
        return "Saturday";
        case 7:
        return "Sunday";
        default:
        return "";

    }
}


}
    

