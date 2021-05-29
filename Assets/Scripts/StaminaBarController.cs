using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(MovementController))]
public class StaminaBarController : MonoBehaviour
{
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TMP_Text staminaText;

    private float stamina;
    private float maxStamina;

    //REFERENCES
    private MovementController movController;

    void Awake()
    {
        movController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Start()
    {
        staminaBar.maxValue = movController.getMaxStamina();
        staminaBar.value = movController.getStamina();
    }
    private void Update()
    {
        staminaBar.value = movController.getStamina();
        if (staminaBar.value == 0)
        {
            staminaText.text = $"{System.Math.Round(movController.getCooldown() *-1)}s. cooldown";
        }
        else
        {
            staminaText.text = "Stamina";
        }
    }

}
