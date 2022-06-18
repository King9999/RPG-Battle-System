using UnityEngine;
using TMPro;

//displays information about selected hero.
public class HeroDetails : MonoBehaviour
{
    public TextMeshProUGUI heroInfo;

    public static HeroDetails instance;

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
        ShowWindow(false);
    }

    public void ShowWindow(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void ShowHeroDetails(string details)
    {
        heroInfo.text = details;
    }
}
