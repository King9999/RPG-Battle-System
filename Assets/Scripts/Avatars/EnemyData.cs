using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Avatar/Enemy", fileName = "enemy_")]
public class EnemyData : AvatarData
{
    public int maxShieldTokens = 0;
    public int xp;
    public int money;
    public Item commonItemDrop;
    public Item rareItemDrop;
    public float commonItemDropChance;
    public float rareItemDropChance;
}
