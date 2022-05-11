using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles distribution of items in treasure chests. */
public class LootManager : MonoBehaviour
{
    LootTables lootTable;
    public TextAsset tableFile;
    public static LootManager instance;

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
        lootTable = JsonUtility.FromJson<LootTables>(tableFile.text);
    }

   
}
