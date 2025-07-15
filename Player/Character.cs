using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

//used for the stats
public class stats
{
    public int maxValue;
    public int currentValue;

    public stats(int current, int max){
       currentValue = current;
       maxValue = max;
    }

    internal void Subtract(int amount){
        currentValue -= amount;
    }

    internal void setNumber(int amount)
    {
        currentValue = amount;
    }
}

public class LevelStats
{
    public static LevelStats instance;
    public string skill;
    public int level;
    private SO_Levels so_levels;
    public int[] xpToLevel;
    public int basexp = 0;
    public int exp;

    private void Awake()
    {
        instance = this;
    }
    public LevelStats(String skillname,SO_Levels so_levels)
    {
        this.so_levels = so_levels;
        xpToLevel = so_levels.getIntArrayLevels();
        skill = skillname;
        level = 1;
        exp = 0;

    }

    public void addXp(int xpGained)
    {
        exp += xpGained;
        if (exp > xpToLevel[level-1])
        {
            this.level++;
        }
    }

}
//character stats
public class Character : MonoBehaviour
{
    //public int[] xpToLevel;
    public static Character instance;
    public stats health;
    public stats energy;
    public LevelStats woodcutting;
    public LevelStats mining;
    [SerializeField] private SO_Levels so_levels = null;

    [SerializeField] StatBar hpBar;
    [SerializeField] StatBar energyBar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //hpBar.setBar(health.currentValue, health.maxValue);
        
        energyBar.setBar(energy.currentValue, energy.maxValue);
        woodcutting = new LevelStats("Woodcutting",so_levels);
        mining = new LevelStats("mining",so_levels);
        hpBar.setBar(woodcutting.exp, woodcutting.xpToLevel[woodcutting.level-1]);
    }

    public void updateBar()
    {
        energyBar.setBar(energy.currentValue, energy.maxValue);
        hpBar.setBar(woodcutting.exp, woodcutting.xpToLevel[woodcutting.level-1]);
    }

    public void wcAction(int xp)
    {
        woodcutting.addXp(xp);
        useEnergy(5);
    }

    public void miningAction(int xp)
    {
        mining.addXp(xp);
        useEnergy(5);
    }

    public void useEnergy(int amount)
    {
        energy.Subtract(amount);
        updateBar();
    }



}