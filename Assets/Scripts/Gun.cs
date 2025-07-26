﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public GameObject bullet;

    public bool canAutoFire;

    public float fireRate;
    [HideInInspector]
    public float fireCounter;

    public int currentAmmo;
    public int maximumAmmo;
    public int pickupAmount;

    public Transform firepoint;

    public float zoomAmount;

    public string gunName;




    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }


    public void GetAmmo()
    {
        currentAmmo += pickupAmount;


        UIController.uiController.ammoSlider.value = currentAmmo;

        float ammoPercentage = (float)currentAmmo / maximumAmmo * 100f;

        UIController.uiController.ammoText.text = $"{ammoPercentage}%" + currentAmmo;
    }


} // end of class
