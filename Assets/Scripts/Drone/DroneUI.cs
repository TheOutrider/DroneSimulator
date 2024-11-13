using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private GameObject drone;
    private DroneController droneController;

    [SerializeField]
    private GameObject FPC, TPC, FPCanvas, TPCanvas;

    private bool cameraSwap = false, hasLauncherAcquired = false;

    private void Start()
    {
        drone = GameObject.FindWithTag("Player");
        if (drone != null)
        {
            droneController = drone.gameObject.GetComponent<DroneController>();
        }
        DroneController.OnLauncherAcquired += OnLauncherAcquired;
    }

    private void OnLauncherAcquired()
    {
        hasLauncherAcquired = true;
    }

    private void Update()
    {
        HandleCameraPOV();
        UpdateHealthBar(droneController.health, 200f);
    }

    private void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void HandleCameraPOV()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraSwap = !cameraSwap;
            FPC.SetActive(cameraSwap);
            FPCanvas.SetActive(cameraSwap);
            TPC.SetActive(!cameraSwap);
            TPCanvas.SetActive(!cameraSwap);
        }
    }


}
