using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(MovementController))]


public class Player : MonoBehaviour
{

    [SerializeField, Min(0)] private int points = 0;
    [SerializeField, Min(0)] private int healthPoints = 1;
    [SerializeField, Min(1)] private int maxHealthPoints = 3;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text healthPointsText;
    [SerializeField] private Slider staminaBar;
    
    [SerializeField] UnityEvent onStopGame;
    [SerializeField] UnityEvent onWinGame;


    private MovementController movementController;
    
    private GameplayCanvasController gameplayController;

    AudioSource playerAudioSource;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        
 
    }
    private void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
        pointsText.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(true);
        healthPointsText.gameObject.SetActive(true);
        UpdatePointsText();
        UpdateHealthPointsText();
    }

    public void AddPoints(int value)
    {
        points += value;
        UpdatePointsText();
    }
    public void AddHealthPoint(int value)
    {
        if(healthPoints < maxHealthPoints)
        {
            healthPoints += value;
            UpdateHealthPointsText();
        }
    }
    public void removeHealthPoint()
    {
        if(healthPoints > 0)
        {
            healthPoints -= 1;
            UpdateHealthPointsText();
        }
        else
        {
            Lose();
        }
    }

    private void UpdatePointsText()
    {
        pointsText.text = $"Points: {points}";
    }    

    private void UpdateHealthPointsText()
    {
        healthPointsText.text = $"Lives: {healthPoints}";
    }

    public void Win()
    {
        onWinGame.Invoke();
    }    
    public void Lose()
    {
        
        onStopGame.Invoke();
    }
    private void PlayerFootstepSound()
    {
        playerAudioSource.Play();
    }

    public int getPoints()
    {
        return points;
    }
    public int getHealthPoints()
    {
        return healthPoints;
    }

}
