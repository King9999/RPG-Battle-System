using UnityEngine;
using TMPro;

/* Displays a hero's stats when equipping new gear. Shows both current and updated stats. */
public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI currentValuesUI; //display order: maxHP, maxMP, ATP, DFP, SPD, MAG, RES, skill.
    public TextMeshProUGUI newValuesUI;
    public TextMeshProUGUI itemToEquipUI;
    string upColor;          //changes the color of values when there's a difference in stats
    string downColor;
    string endColor;

    // Start is called before the first frame update
    void Start()
    {
        ShowDisplay(false);
        currentValuesUI.text = "";
        newValuesUI.text = "";
        itemToEquipUI.text = "";
        upColor = "<color=#5EFF41>";
        downColor = "<color=#B22626>";
        endColor = "</color>";
    }

    public void ShowDisplay(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void ClearDisplay()
    {
        currentValuesUI.text = "";
        newValuesUI.text = "";
        itemToEquipUI.text = "";
    }

    public void UpdateStats(Hero hero, Item newItem)
    {
        //get item type
        itemToEquipUI.text = newItem.itemName;
        Weapon newWeapon = null;
        Armor newArmor = null;
        Trinket newTrinket = null;
        switch(newItem.itemType)
        {
            case Item.ItemType.Weapon:
                newWeapon = (Weapon)newItem;
                break;

            case Item.ItemType.Armor:
                newArmor = (Armor)newItem;
                break;

            case Item.ItemType.Trinket:
                newTrinket = (Trinket)newItem;
                break;

        }

        //show hero's current values
        currentValuesUI.text = hero.maxHitPoints + "\n" + hero.maxManaPoints + "\n" + hero.atp +"\n" + hero.dfp + "\n" + hero.spd + "\n" +
                                hero.mag + "\n" + hero.res + "\n";

        //check for any available skills, and then get the new values
        if (newWeapon != null)
        {
            if (hero.weapon.weaponSkill == null)
                    currentValuesUI.text += "No";
                else
                    currentValuesUI.text += hero.weapon.weaponSkill.skillName;
                   
            //change value colour if there's a difference in values
            string newAtp;
            float difference = (hero.atp - hero.weapon.atp) + newWeapon.atp;
            if (difference > hero.atp)
                newAtp = upColor + difference + endColor;
            else if (difference < hero.atp)
                newAtp = downColor + difference + endColor;
            else
                newAtp = hero.atp.ToString();
            
            string newMag;
            difference = (hero.mag - hero.weapon.mag) + newWeapon.mag;
            if (difference > hero.mag)
                newMag = upColor + difference + endColor;
            else if (difference < hero.mag)
                newMag = downColor + difference + endColor;
            else
                newMag = hero.mag.ToString();

            //does new item have a skill?
            string newSkill;
            if (newWeapon.weaponSkill == null)
                    newSkill = "No";
                else
                    newSkill = newWeapon.weaponSkill.skillName;


            newValuesUI.text = hero.maxHitPoints + "\n" + hero.maxManaPoints + "\n" + newAtp +"\n" + hero.dfp + "\n" + hero.spd + "\n" +
                                newMag + "\n" + hero.res + "\n" + newSkill;
        }

        if (newArmor != null)
        {
             if (hero.armor.armorSkill == null)
                    currentValuesUI.text += "No";
                else
                    currentValuesUI.text += hero.armor.armorSkill.skillName;
        } 

        if (newTrinket != null)
        {
             if (hero.trinket.trinketSkill == null)
                    currentValuesUI.text += "No";
                else
                    currentValuesUI.text += hero.trinket.trinketSkill.skillName;
        }

    }
}
