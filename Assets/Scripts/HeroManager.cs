using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<Hero> heroes;
    public Hero heroPrefab;
    public HeroData[] heroData;           //this should be a data manager
    

    public static HeroManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddHero(heroData[0]);
        AddHero(heroData[1]);
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
        heroes.Add(hero);
    }
}
