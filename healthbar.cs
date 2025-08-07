using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class healthbar : MonoBehaviour
{
    [SerializeField] private health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;


    private void Start()
    {
        // anything
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
