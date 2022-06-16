using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is mainly used to pass data over to the game scene.
public class TitleManager : MonoBehaviour
{
    public Sprite selectedHeroSprite;
    public HeroData selectedHeroData;
    //public Hero heroPrefab;
    //public HeroData barbData;
    //public HeroData rogueData;
    //public HeroData 

    public static TitleManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        #if UNITY_EDITOR_WIN
            Debug.Log("Screen test");
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        #endif
    }
}
