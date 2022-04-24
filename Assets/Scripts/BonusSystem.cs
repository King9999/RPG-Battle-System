using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* This script handles any bonuses the player receives after breaking enemy shields. The received bonuses are random, but the exceptionally powerful
   bonuses have a small chance of occurring. All bonuses are stored in a list, and when they're obtained, they're removed from the list so they can't be acquired
   again. 
   
   BONUSES (temporary bonuses last until bonus turns end)
    All panels except Special become Critical panels
    Party is fully restored, ailments removed, and dead heroes are revived (rarest bonus). Occurs once per battle
    Bonus EXP and money (does not stack)
    100% chance of a rare drop from all enemies that have rare drops.
    Enemy buffs are removed. Attempts to buff fail while bonus turns are active
    Action Gauge is slowed down considerably
    All heroes gain a 20% buff to all stats (except HP and MP) while bonus turns are active
    Skills cost 0 mana
   */
public class BonusSystem : MonoBehaviour
{
    public enum BonusValue {AllCriticalPanels, BonusXpAndMoney, RareDropBonus, EnemyBuffsRemoved, ActionGaugeSlowed, BuffAllStats, NoCostToSkills, FullRestore}
    public List<BonusValue> bonusValues;
    public List<BonusValue> activeBonuses;              //bonuses that player has received

    public static BonusSystem instance;
    UI ui;
    CombatSystem cs;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        ui = UI.instance;
        cs = CombatSystem.instance;
        ResetBonuses();
    }

    public void GetBonus(BonusValue bonus)
    {
        if ((int)bonus < 0 || (int)bonus >= Enum.GetNames(typeof(BonusValue)).Length) return;

        activeBonuses.Add(bonus);

        //Give bonuses and update UI. Certain bonuses are removed from the bonusValues list since they only occur once per battle.
        switch(bonus)
        {
            case BonusValue.AllCriticalPanels:
                ui.bonusListUI.text += "All Panels Critical\n";
                break;

            case BonusValue.BonusXpAndMoney:
                ui.bonusListUI.text += "Bonus EXP & Money\n";
                bonusValues.RemoveAt((int)bonus);
                break;

            case BonusValue.RareDropBonus:
                ui.bonusListUI.text += "100% Rare Item Drop\n";
                break;

            case BonusValue.EnemyBuffsRemoved:
                ui.bonusListUI.text += "Enemy Buffs Disabled\n";
                break;

            case BonusValue.ActionGaugeSlowed:
                ui.bonusListUI.text += "Action Gauge Speed Reduced\n";
                break;

            case BonusValue.BuffAllStats:
                ui.bonusListUI.text += "All Heroes' Stats Buffed\n";
                break;

            case BonusValue.NoCostToSkills:
                ui.bonusListUI.text += "Skills Cost 0 MP\n";
                break;

            case BonusValue.FullRestore:
                ui.bonusListUI.text += "Full Restore Activated\n";
                bonusValues.RemoveAt((int)bonus);
                break;
        }  
    }

    public void RemoveBonus(BonusValue bonus)
    {
        if ((int)bonus < 0 || (int)bonus >= Enum.GetNames(typeof(BonusValue)).Length) return;

        activeBonuses.RemoveAt((int)bonus);

        //update UI
        ui.bonusListUI.text = "<color=#c827d8>BONUSES</color>\n";

        foreach(BonusValue bonusValue in activeBonuses)
        {
            switch(bonusValue)
            {
                case BonusValue.AllCriticalPanels:
                ui.bonusListUI.text += "All Panels Critical\n";
                break;

                case BonusValue.BonusXpAndMoney:
                    ui.bonusListUI.text += "Bonus EXP & Money\n";
                    break;

                case BonusValue.RareDropBonus:
                    ui.bonusListUI.text += "100% Rare Item Drop\n";
                    break;

                case BonusValue.EnemyBuffsRemoved:
                    ui.bonusListUI.text += "Enemy Buffs Disabled\n";
                    break;

                case BonusValue.ActionGaugeSlowed:
                    ui.bonusListUI.text += "Action Gauge Speed Reduced\n";
                    break;

                case BonusValue.BuffAllStats:
                    ui.bonusListUI.text += "All Heroes' Stats Buffed\n";
                    break;

                case BonusValue.NoCostToSkills:
                    ui.bonusListUI.text += "Skills Cost 0 MP\n";
                    break;

                case BonusValue.FullRestore:
                    ui.bonusListUI.text += "Full Restore Activated\n";
                    break;
            }
        }

    }

    public void ResetBonuses()
    {
        bonusValues.Clear();
        activeBonuses.Clear();
        ui.bonusListUI.text = "<color=#c827d8>BONUSES</color>\n";

        for (int i = 0; i < Enum.GetNames(typeof(BonusValue)).Length; i++)
        {
            bonusValues.Add((BonusValue)i);
        }
    }
}
