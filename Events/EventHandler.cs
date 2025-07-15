using System;
using System.Collections.Generic;

public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, 
ToolEffect toolEffect,
bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
bool idleUp, bool idleDown, bool idleLeft, bool idleRight);
public static class EventHandler
{

  //inventory eventhandler
  public static event Action<InvLoc, List<InvItem>> InvUpdateEvent;

  public static void CallInvUpdateEvent(InvLoc invLoc, List<InvItem> invList)
  {
  if(InvUpdateEvent !=null){
  InvUpdateEvent(invLoc,invList);}
  
  }


    // Drop selected item event
    public static event Action DropSelectedItemEvent;

    public static void CallDropSelectedItemEvent()
    {
        if (DropSelectedItemEvent != null)
            DropSelectedItemEvent();
    }
    //movement eventhandler
    public static event MovementDelegate MovementEvent;

  //movement event call for publishers
  public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, 
ToolEffect toolEffect,
bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
{
  if(MovementEvent!=null){
      MovementEvent(inputX,inputY,isWalking,isRunning,isIdle,isCarrying,toolEffect,isUsingToolRight,isUsingToolLeft,isUsingToolUp,isUsingToolDown,
      isLiftingToolRight,isLiftingToolLeft,isLiftingToolUp,isLiftingToolDown,isPickingRight,isPickingLeft,isPickingUp,isPickingDown,isSwingingToolRight,
      isSwingingToolLeft,isSwingingToolUp,isSwingingToolDown,idleUp,idleDown,idleLeft,idleRight);
  }
}

//time events
public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason,int gameDay, String gameDayWeek, int gameHour, int gameMinute, int gameSecond){
  if(AdvanceGameMinuteEvent !=null){
    AdvanceGameMinuteEvent(gameYear, gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }
}

public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;

public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason,int gameDay, String gameDayWeek, int gameHour, int gameMinute, int gameSecond){
  if(AdvanceGameHourEvent !=null){
    AdvanceGameHourEvent(gameYear, gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }
}

public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;

public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason,int gameDay, String gameDayWeek, int gameHour, int gameMinute, int gameSecond){
  if(AdvanceGameDayEvent !=null){
    AdvanceGameDayEvent(gameYear, gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }
}

public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;

public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason,int gameDay, String gameDayWeek, int gameHour, int gameMinute, int gameSecond){
  if(AdvanceGameSeasonEvent !=null){
    AdvanceGameSeasonEvent(gameYear, gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }
}

public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;

public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason,int gameDay, String gameDayWeek, int gameHour, int gameMinute, int gameSecond){
  if(AdvanceGameYearEvent !=null){
    AdvanceGameYearEvent(gameYear, gameSeason,gameDay,gameDayWeek,gameHour,gameMinute,gameSecond);
  }
}

    //creates trees and rocks on start
    public static event Action InstantiateResourcePrefabsEvent;
    public static void CallInstantiateResourcePrefabsEvent()
    {
        if(InstantiateResourcePrefabsEvent !=null)
        {
            InstantiateResourcePrefabsEvent();

        }
    }


//scene manager related events
public static event Action BeforeSceneUnloadFadeOutEvent;

public static void CallBeforeSceneUnloadFadeOutEvent(){
  if(BeforeSceneUnloadFadeOutEvent!=null){
    BeforeSceneUnloadFadeOutEvent();
  }
}

public static event Action BeforeSceneUnloadEvent;

public static void CallBeforeSceneUnloadEvent(){
  if(BeforeSceneUnloadEvent!=null){
    BeforeSceneUnloadEvent();
  }
}

public static event Action AfterSceneLoadEvent;

public static void CallAfterSceneLoadEvent(){
  if(AfterSceneLoadEvent!=null){
    AfterSceneLoadEvent();
  }
}

public static event Action AfterSceneLoadFadeInEvent;

public static void CallAfterSceneLoadFadeInEvent(){
  if(AfterSceneLoadFadeInEvent!=null){
    AfterSceneLoadFadeInEvent();
  }
}


}


