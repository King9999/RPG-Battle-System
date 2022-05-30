using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//take item from target if they have one. Normal = steal common item, Critical = steal rare item
[CreateAssetMenu(menuName = "Skill/Rogue/Steal", fileName = "skill_steal")]
public class Steal : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float baseChance = user.atp;
       
        ReduceMp(user);
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Miss:
                    ui.DisplayStatusUpdate("MISS", target.transform.position);
                    break;

                case ActionGauge.ActionValue.Normal:
                    if (target.TryGetComponent(out Enemy enemy))
                    {
                        if (enemy.commonItemDrop != null)
                        {
                            Inventory inv = Inventory.instance;
                            if (enemy.commonItemDrop.itemType == Item.ItemType.Consumable)
                                inv.AddItem((Consumable)enemy.commonItemDrop, 1);
                            else if (enemy.commonItemDrop.itemType == Item.ItemType.Weapon)
                                inv.AddItem((Weapon)enemy.commonItemDrop, 1);
                            else if (enemy.commonItemDrop.itemType == Item.ItemType.Armor)
                                inv.AddItem((Armor)enemy.commonItemDrop, 1);
                            else
                                inv.AddItem((Trinket)enemy.commonItemDrop, 1);
                            
                            //remove item so it can't be stolen again
                            Debug.Log("Stole " + enemy.commonItemDrop.itemName);
                            enemy.commonItemDrop = null;
                            ui.DisplayStatusUpdate("STOLE COMMON ITEM", target.transform.position);
                        }
                        else
                            ui.DisplayStatusUpdate("NO ITEM", target.transform.position);
                    }
                    break;

                case ActionGauge.ActionValue.Critical:
                    if (target.TryGetComponent(out Enemy targetEnemy))
                    {
                        if (targetEnemy.rareItemDrop != null)
                        {
                            Inventory inv = Inventory.instance;
                            if (targetEnemy.rareItemDrop.itemType == Item.ItemType.Consumable)
                                inv.AddItem((Consumable)targetEnemy.rareItemDrop, 1);
                            else if (targetEnemy.rareItemDrop.itemType == Item.ItemType.Weapon)
                                inv.AddItem((Weapon)targetEnemy.rareItemDrop, 1);
                            else if (targetEnemy.rareItemDrop.itemType == Item.ItemType.Armor)
                                inv.AddItem((Armor)targetEnemy.rareItemDrop, 1);
                            else
                                inv.AddItem((Trinket)targetEnemy.rareItemDrop, 1);
                            
                            //remove item so it can't be stolen again
                            Debug.Log("Stole " + targetEnemy.rareItemDrop.itemName);
                            targetEnemy.rareItemDrop = null;
                            ui.DisplayStatusUpdate("STOLE RARE ITEM", target.transform.position);
                        }
                        else
                            ui.DisplayStatusUpdate("NO ITEM", target.transform.position);
                    }
                    break;
                
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}
