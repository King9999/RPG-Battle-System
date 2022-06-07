using UnityEngine;
using TMPro;

/* Displays hero's stats and equipment when mouse is placed over party UI. */
public class HeroStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI statValuesUI;    //displays the following in order: ATP, DFP, SPD, MAG, RES
    public TextMeshProUGUI equipmentUI;     //displays in order: weapon, armor, trinket
    public TextMeshProUGUI skillsUI;
    // Start is called before the first frame update
    void Start()
    {
        //ShowDisplay(false);
        statValuesUI.text = "";
        equipmentUI.text = "";
        skillsUI.text = "";
    }

    public void ShowDisplay(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void UpdateStats(Hero hero)
    {
        //display stats
        hero.UpdateStats();
        statValuesUI.text = hero.atp + "\n" + hero.dfp + "\n" + hero.spd + "\n" + hero.mag + "\n" + hero.res;

        //display equipment
        if (hero.weapon.weaponType == Weapon.WeaponType.Staff)
            equipmentUI.text = hero.weapon.itemName + "(" + hero.weapon.weaponSkill.skillName + ")";
        else
            equipmentUI.text = hero.weapon.itemName;
        equipmentUI.text += (hero.armor == null) ? "\n<NO ARMOR>" : "\n" + hero.armor.itemName;
        equipmentUI.text += (hero.trinket == null) ? "\n<NO TRINKET>" : "\n" + hero.trinket.itemName;

        //display skills
        if (hero.skills.Count <= 0)
            skillsUI.text = "<NONE>";
        else
        {
            skillsUI.text = "";
            foreach(Skill skill in hero.skills)
            {
                skillsUI.text += skill.skillName + "\n";
            }
        }
    }
}
