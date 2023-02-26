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

    #region DamageHandeling Vars

    #endregion

    #region Health Vars
    [Header("Health")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearts;
    [SerializeField] private Sprite emptyHearts;

    private int currentHealth;
    private int numbOfHeartContainers;
    #endregion

    #region Data
    [Header("Data")]
    [SerializeField] private PlayerCombatData playerData;
    #endregion

    #endregion

    #region UnityMethods
    public void Awake()
    {
        currentHealth = (int)playerData.maxHealth;
        numbOfHeartContainers = (int)playerData.maxHealth;
        HealthSpriteFinder();
    }

    public void Update()
    {
        HealthUpdater();
    }
    #endregion

    #region healthMethods
    public void HealthUpdater()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

            currentHealth = currentHealth > numbOfHeartContainers ? numbOfHeartContainers : currentHealth;

            hearts[i].sprite = i < currentHealth ? fullHearts : emptyHearts;

            hearts[i].enabled = i < numbOfHeartContainers;
        }
    }

    public void HealthSpriteFinder()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = GameObject.Find("Heart " + "(" + i + ")").GetComponent<Image>();
        }
    }
    #endregion
}
