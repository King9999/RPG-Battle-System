using UnityEngine;
using TMPro;

/* Displays a hero's stats when equipping new gear. Shows both current and updated stats. */
public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI currentValuesUI; //display order: maxHP, maxMP, ATP, DFP, SPD, MAG, RES, skill.
    public TextMeshProUGUI newValuesUI;
    public TextMeshProUGUI itemToEquipUI;
    public TextMeshProUGUI equippedItemUI;
    string upColor;          //changes the color of values when there's a difference in stats
    string downColor;
    string endColor;

    // Start is called before the first frame update
    void Start()
    {
        ShowDisplay(false);
        ClearDisplay();
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
        equippedItemUI.text = "";
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
                equippedItemUI.text = hero.weapon.itemName;
                break;

            case Item.ItemType.Armor:
                newArmor = (Armor)newItem;
                equippedItemUI.text = hero.armor == null ? "<NONE>" : hero.armor.itemName;
                break;

            case Item.ItemType.Trinket:
                newTrinket = (Trinket)newItem;
                equippedItemUI.text = hero.trinket == null ? "<NONE>" : hero.trinket.itemName;
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
             if (hero.armor != null)
             {
                 if (hero.armor.armorSkill == null)
                    currentValuesUI.text += "No";
                else
                    currentValuesUI.text += hero.armor.armorSkill.skillName;  
             }
             else
             {
                 currentValuesUI.text += "No";
             }
                    
                
            
            //change value colour if there's a difference in values
            string newDfp;
            float heroDfp = hero.armor == null ? 0 : hero.armor.dfp;        //need to do this in case no armor is present.
            float difference = (hero.dfp - heroDfp) + newArmor.dfp;
            if (difference > hero.dfp)
                newDfp = upColor + difference + endColor;
            else if (difference < hero.dfp)
                newDfp = downColor + difference + endColor;
            else
                newDfp = hero.dfp.ToString();
            
            string newRes;
            float heroRes = hero.armor == null ? 0 : hero.armor.res;
            difference = (hero.res - heroRes) + newArmor.res;
            if (difference > hero.res)
                newRes = upColor + difference + endColor;
            else if (difference < hero.res)
                newRes = downColor + difference + endColor;
            else
                newRes = hero.res.ToString();

            //does new item have a skill?
            string newSkill;
            if (newArmor.armorSkill == null)
                    newSkill = "No";
                else
                    newSkill = newArmor.armorSkill.skillName;


            newValuesUI.text = hero.maxHitPoints + "\n" + hero.maxManaPoints + "\n" + hero.atp +"\n" + newDfp + "\n" + hero.spd + "\n" +
                                hero.mag + "\n" + newRes + "\n" + newSkill;
        } 

        if (newTrinket != null)
        {
             if (hero.trinket != null)
             {
                 if (hero.trinket.trinketSkill == null)
                    currentValuesUI.text += "No";
                else
                    currentValuesUI.text += hero.trinket.trinketSkill.skillName;
             }
             else
                currentValuesUI.text += "No";
            
            //change value colour if there's a difference in values
            //hit points
            string newHp;
            float heroHp = hero.trinket == null ? 0 : hero.trinket.maxHitPoints;
            float difference = (hero.maxHitPoints - heroHp) + newTrinket.maxHitPoints;
            if (difference > hero.maxHitPoints)
                newHp = upColor + difference + endColor;
            else if (difference < hero.maxHitPoints)
                newHp = downColor + difference + endColor;
            else
                newHp = hero.maxHitPoints.ToString();
            
            //mana points
            string newMp;
            float heroMp = hero.trinket == null ? 0 : hero.trinket.maxManaPoints;
            difference = (hero.maxManaPoints - heroMp) + newTrinket.maxManaPoints;
            if (difference > hero.maxManaPoints)
                newMp = upColor + difference + endColor;
            else if (difference < hero.maxManaPoints)
                newMp = downColor + difference + endColor;
            else
                newMp = hero.maxManaPoints.ToString();

            //atp
            string newAtp;
            float heroAtp = hero.trinket == null ? 0 : hero.trinket.atp;
            difference = (hero.atp - heroAtp) + newTrinket.atp;
            if (difference > hero.atp)
                newAtp = upColor + difference + endColor;
            else if (difference < hero.atp)
                newAtp = downColor + difference + endColor;
            else
                newAtp = hero.atp.ToString();
            
            //mag
            string newMag;
            float heroMag = hero.trinket == null ? 0 : hero.trinket.mag;
            difference = (hero.mag - heroMag) + newTrinket.mag;
            if (difference > hero.mag)
                newMag = upColor + difference + endColor;
            else if (difference < hero.mag)
                newMag = downColor + difference + endColor;
            else
                newMag = hero.mag.ToString();
            
            //spd
            string newSpd;
            float heroSpd = hero.trinket == null ? 0 : hero.trinket.spd;
            difference = (hero.spd - heroSpd) + newTrinket.spd;
            if (difference > hero.spd)
                newSpd = upColor + difference + endColor;
            else if (difference < hero.spd)
                newSpd = downColor + difference + endColor;
            else
                newSpd = hero.spd.ToString();

            //dfp
            string newDfp;
            float heroDfp = hero.trinket == null ? 0 : hero.trinket.dfp;
            difference = (hero.dfp - heroDfp) + newTrinket.dfp;
            if (difference > hero.dfp)
                newDfp = upColor + difference + endColor;
            else if (difference < hero.dfp)
                newDfp = downColor + difference + endColor;
            else
                newDfp = hero.dfp.ToString();
            
            //res
            string newRes;
            float heroRes = hero.trinket == null ? 0 : hero.trinket.res;
            difference = (hero.res - heroRes) + newTrinket.res;
            if (difference > hero.res)
                newRes = upColor + difference + endColor;
            else if (difference < hero.res)
                newRes = downColor + difference + endColor;
            else
                newRes = hero.res.ToString();

            //does new item have a skill?
            string newSkill;
            if (newTrinket.trinketSkill == null)
                    newSkill = "No";
                else
                    newSkill = newTrinket.trinketSkill.skillName;


            newValuesUI.text = newHp + "\n" + newMp + "\n" + newAtp +"\n" + newDfp + "\n" + newSpd + "\n" +
                                newMag + "\n" + newRes + "\n" + newSkill;
        }

    }
}
