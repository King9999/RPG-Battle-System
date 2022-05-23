using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathState {Normal, Locked}
    public PathState pathState;
   
    public void ShowPath(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public bool PathVisible() {  return gameObject.activeSelf;}
}
