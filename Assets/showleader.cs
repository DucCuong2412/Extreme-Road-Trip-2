using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showleader : MonoBehaviour
{
    public GameObject leaderboard;
  
    public void ShowLeaderboard()
    {
        if (leaderboard != null)
        {
            leaderboard.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Leaderboard GameObject is not assigned.");
        }
    }
}
