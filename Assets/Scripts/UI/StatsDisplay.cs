using UnityEngine;
using TMPro;

/* Displays a hero's stats when equipping new gear. Shows both current and updated stats. */
public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI currentValuesUI;
    public TextMeshProUGUI newValuesUI;
    public TextMeshProUGUI itemToEquipUI;

    // Start is called before the first frame update
    void Start()
    {
        ShowDisplay(false);
    }

    public void ShowDisplay(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
