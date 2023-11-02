using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    TextMeshProUGUI LevelUp;

    void Start()
    {
        LevelUp = GetComponentInChildren<TextMeshProUGUI>();
        LevelUp.enabled = false;
    }

    public IEnumerator ShowText(string levelInstructions, int level)
    {
        LevelUp.enabled = true;
        LevelUp.text = "Level Up!!";
        yield return new WaitForSeconds(1.5f);
        LevelUp.text = "You are now XP level " + level + ". " + levelInstructions;
        yield return new WaitForSeconds(3.0f);
        LevelUp.enabled = false;
    }
}
