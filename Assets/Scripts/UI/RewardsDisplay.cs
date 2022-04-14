using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardsDisplay : MonoBehaviour
{
    public Image border;
    public TextMeshProUGUI rewardsUI;           //displays xp and money awarded. XP shown is the total amount
    public TextMeshProUGUI[] partyXpUI;         //displays the the divvied xp and remaining xp required to level up for each hero
    public TextMeshProUGUI lootUI;              //displays awarded items.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowRewards(string xpAndMoney, string[] partyXp, string loot)
    {
        gameObject.SetActive(true);

        rewardsUI.text = xpAndMoney;

       for (int i = 0; i < partyXp.Length; i++)
       {
           partyXpUI[i].text = partyXp[i];
       }

       lootUI.text = loot;

       //run a coroutine that shows the party's current xp being reduced to the remaining xp
    }

    public void HideDisplay()
    {
        rewardsUI.text = "";
        lootUI.text = "";
        foreach(TextMeshProUGUI xpText in partyXpUI)
        {
            xpText.text = "";
        }
        gameObject.SetActive(false);
    }
}
