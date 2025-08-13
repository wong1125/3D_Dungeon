using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private float initialHealth = 100;
    public float InitialHealth { get { return initialHealth; } }
    private float maxHealth = 100;
    public float MaxHealth {  get { return maxHealth; } }
    private float currentHealth;

    private float initialStamina = 100;
    public float InitialStamina { get { return initialStamina; } }
    private float maxStamina = 100;
    public float MaxStamina { get { return maxStamina; } }
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    private float previousChangedTime;
    private float staminaChageRate = 0.05f;
    
    public Action<float> HealthChanger;
    public Action<float> StaminaChanger;

    private Rigidbody rb;
    private PlayerController playerController;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        currentHealth = initialHealth;
        HealthChanger += ChangeHealth;
        currentStamina = initialStamina;
        StaminaChanger += ChangeStamina;
    }

    private void Update()
    {
        if (playerController.RunSwitch == 1 && Time.time - previousChangedTime > staminaChageRate)
        {
            StaminaChanger(-1);
            previousChangedTime = Time.time;
        }
        else if (playerController.IsMoving == false && Time.time - previousChangedTime > staminaChageRate)
        {
            StaminaChanger(0.5f);
            previousChangedTime = Time.time;
        }

    }

    void ChangeHealth(float value)
    {
        currentHealth += value;
        if (currentHealth >= maxStamina)
            currentHealth = maxStamina;
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());

        }
    }

    void ChangeStamina(float value)
    {
        currentStamina += value;
        if (currentStamina >= maxHealth)
            currentStamina = maxHealth;
        else if (currentStamina <= 0)
        {
            currentStamina = 0;
            playerController.BanRunning();
        }
    }

    IEnumerator Die()
    {
        transform.position = Vector3.zero;
        yield return null;
        HealthChanger(initialHealth);
    }
}
