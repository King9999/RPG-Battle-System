using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Action tokens travel along the action gauge, moving right to left. */
public class ActionToken : MonoBehaviour
{
    public float moveSpeed;         //how fast it travels along the action gauge;

    //public Image token;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 300;
    }

    public void StopToken()
    {
        moveSpeed = 0;
    }
}
