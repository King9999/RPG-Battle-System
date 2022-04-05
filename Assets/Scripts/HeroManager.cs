using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<Hero> heroes;
    public Hero heroPrefab;
    public HeroData data;           //this should be a data manager

    // Start is called before the first frame update
    void Start()
    {
        AddHero(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHero(HeroData data)
    {
        Hero hero = Instantiate(heroPrefab);
        hero.GetData(data);
        heroes.Add(hero);
    }
}
