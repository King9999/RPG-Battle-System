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
    public bool bonusTurnsActive {get; set;}

    //bonus mods
    public bool allPanelsCritical {get; set;}
    public int xpMoneyMod {get; set;}
    public float statMod {get; set;}
    public float rareDropMod {get; set;}
    public bool enemyBuffsDisabled {get; set;}
    public bool heroBuffsEnabled {get; set;}
    public float actionGaugeMod {get; set;}
    public float manaCostMod {get; set;}

    //singletons
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
        xpMoneyMod = 0;
        statMod = 0.2f;
        actionGaugeMod = 1;
        manaCostMod = 1;
        ResetBonuses();
        //GetBonus(BonusValue.RareDropBonus);
    }

    public void GetBonus(BonusValue bonus)
    {
        if ((int)bonus < 0 || (int)bonus >= Enum.GetNames(typeof(BonusValue)).Length) return;

        activeBonuses.Add(bonus);
        bonusTurnsActive = true;

        //Give bonuses and update UI. Certain bonuses are removed from the bonusValues list since they only occur once per battle.
        switch(bonus)
        {
            case BonusValue.AllCriticalPanels:
                ui.bonusListUI.text += "All Panels Critical\n";
                allPanelsCritical = true;
                break;

            case BonusValue.BonusXpAndMoney:
                ui.bonusListUI.text += "Bonus EXP & Money\n";
                //bonusValues.RemoveAt((int)bonus);
                bonusValues.Remove(bonus);
                xpMoneyMod = 2;
                break;

            case BonusValue.RareDropBonus:
                ui.bonusListUI.text += "100% Rare Item Drop\n";
                rareDropMod = 1;        //this is subtracted from the roll when checking for loot
                break;

            case BonusValue.EnemyBuffsRemoved:
                ui.bonusListUI.text += "Enemy Buffs Disabled\n";
                enemyBuffsDisabled = true;
                foreach(Enemy enemy in cs.enemiesInCombat)
                {
                    enemy.atpMod = 1;
                    enemy.dfpMod = 1;
                    enemy.spdMod = 1;
                    enemy.magMod = 1;
                    enemy.resMod = 1;
                }
                break;

            case BonusValue.ActionGaugeSlowed:
                ui.bonusListUI.text += "Action Gauge Speed Reduced\n";
                actionGaugeMod = 0.7f;
                break;

            case BonusValue.BuffAllStats:
                ui.bonusListUI.text += "All Heroes' Stats Buffed\n";
                if (!heroBuffsEnabled)
                {
                    heroBuffsEnabled = true;
                    foreach(Hero hero in cs.heroesInCombat)
                    {
                        hero.atpMod += statMod;
                        hero.dfpMod += statMod;
                        hero.spdMod += statMod;
                        hero.magMod += statMod;
                        hero.resMod += statMod;
                    }
                }
                break;

            case BonusValue.NoCostToSkills:
                ui.bonusListUI.text += "Skills Cost 0 MP\n";
                manaCostMod = 0;
                break;

            case BonusValue.FullRestore:
                ui.bonusListUI.text += "Full Restore Activated\n";
                //bonusValues.RemoveAt((int)bonus);
                bonusValues.Remove(bonus);
                foreach(Hero hero in cs.heroesInCombat)
                {
                    //fully heal HP and MP, revive dead heroes, and remove staus ailments
                    hero.status = Avatar.Status.Normal;
                    hero.hitPoints = hero.maxHitPoints;
                    hero.manaPoints = hero.maxManaPoints;
                }
                break;
        }  
    }

    //only temporary bonuses are removed
    public void RemoveAllBonuses()
    {
        if (cs.bonusTurns > 0) return;

        bonusTurnsActive = false;

        //update UI
        ui.bonusListUI.text = "<color=#c827d8>BONUSES</color>\n";

        for(int i = 0; i < activeBonuses.Count; i++)
        {
            switch(activeBonuses[i])
            {
                case BonusValue.AllCriticalPanels:
                    activeBonuses.RemoveAt(i);
                    allPanelsCritical = false;
                    break;

                case BonusValue.BonusXpAndMoney:
                    ui.bonusListUI.text += "Bonus EXP & Money\n";
                    break;

                case BonusValue.RareDropBonus:
                    ui.bonusListUI.text += "100% Rare Item Drop\n";
                    break;

                case BonusValue.EnemyBuffsRemoved:
                    activeBonuses.RemoveAt(i);
                    enemyBuffsDisabled = false;
                    break;

                case BonusValue.ActionGaugeSlowed:
                    activeBonuses.RemoveAt(i);
                    actionGaugeMod = 1;
                    break;

                case BonusValue.BuffAllStats:
                    activeBonuses.RemoveAt(i);
                    if (heroBuffsEnabled)
                    {
                        heroBuffsEnabled = false;
                        foreach(Hero hero in cs.heroesInCombat)
                        {
                            hero.atpMod -= statMod;
                            hero.dfpMod -= statMod;
                            hero.spdMod -= statMod;
                            hero.magMod -= statMod;
                            hero.resMod -= statMod;
                        }
                    }
                    break;

                case BonusValue.NoCostToSkills:
                    activeBonuses.RemoveAt(i);
                    manaCostMod = 1;
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
        bonusTurnsActive = false;

        for (int i = 0; i < Enum.GetNames(typeof(BonusValue)).Length; i++)
        {
            bonusValues.Add((BonusValue)i);
        }

        //reset mods
        allPanelsCritical = false;
        xpMoneyMod = 0;
        heroBuffsEnabled = false;
        enemyBuffsDisabled = false;
        manaCostMod = 1;
        actionGaugeMod = 1;
        rareDropMod = 0;
    
        foreach(Hero hero in cs.heroesInCombat)
        {
            hero.atpMod = 1;
            hero.dfpMod = 1;
            hero.spdMod = 1;
            hero.magMod = 1;
            hero.resMod = 1;
        }  
    }
}
