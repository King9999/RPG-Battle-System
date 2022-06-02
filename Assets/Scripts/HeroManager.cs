using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<Hero> heroes;
    public Hero heroPrefab;
    public HeroData[] heroData;           //this should be a data manager
    public Skill[] barbSkills;
    public Skill[] rogueSkills;
    public Skill[] mageSkills;
    public Skill[] clericSkills;
    public int barbData {get;} = 0;
    public int rogueData {get;} = 1;  
    public int mageData {get;} = 2;  
    public int clericData {get;} = 3;      

    public static HeroManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //AddHero(heroData[rogueData]);
    }

    // Start is called before the first frame update
    void Start()
    {
        //AddHero(heroData[0]);
        //AddHero(heroData[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHero(HeroData data)
    {
        Hero hero = Instantiate(heroPrefab);
        hero.data = data;
        hero.GetData(hero.data);
        hero.transform.SetParent(CombatSystem.instance.transform);
        hero.gameObject.SetActive(false);       //disabled by default since they're only active in combat.
        heroes.Add(hero);
    }
}
