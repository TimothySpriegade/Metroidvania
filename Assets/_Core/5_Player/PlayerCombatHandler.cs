using _Core._5_Player.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatHandler : MonoBehaviour
{
    #region vars

    #region health
    [Header("health")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearts;
    [SerializeField] private Sprite emptyHearts;

    private int currentHealth;
    private int numbOfHeartContainers;

    #endregion

    #region data

    [SerializeField] private PlayerCombatData playerData;

    #endregion

    #endregion

    #region UnityMethods
    public void Awake()
    {
        currentHealth = (int)playerData.maxHealth;
        numbOfHeartContainers = (int)playerData.maxHealth;
        for(int i = 0; i < hearts.Length; i++) {
            //hearts[i] = GameObject.Find("Heart" + "(" + i + ")");
            hearts[i] = GameObject.Find("Heart " + "(" + i + ")").GetComponent<Image>();
        }
    }

    public void Update()
    {
        HealthHandler();
    }
    #endregion

    public void HealthHandler()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

            if (currentHealth > numbOfHeartContainers)
            {
                currentHealth = numbOfHeartContainers;
            }

            if (i < currentHealth)
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }

            if (i < numbOfHeartContainers)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
