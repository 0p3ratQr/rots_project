using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBarUI : MonoBehaviour
{
    Image healthBar;
    public Player player;
    public float maxHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthBar = GetComponent<Image>();
        maxHealth = player.maxHealth;
    }
    void Update()
    {
        healthBar.fillAmount = (float)player.GetCurrentHealth() / maxHealth;
    }
}