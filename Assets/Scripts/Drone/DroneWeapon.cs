using UnityEngine;
using TMPro;
using System;

public class DroneWeapon : MonoBehaviour
{
    public int gunAmmo = 100, fireballAmmo = 0;

    [SerializeField] private TextMeshProUGUI gunAmmoTextFP, fireballAmmoFP, gunAmmoTextTP, fireballAmmoTP;
    [SerializeField] private GameObject gunAmmoUI, fireballAmmoUI, gunAmmoUIFP, fireballAmmoUIFP;

    [Header("Drone Audios")]
    [SerializeField] private AudioSource droneAudioSource;
    [SerializeField] private AudioClip turret, emptyGun;

    private void Start()
    {
        DroneController.OnGunFired += GunFired;
        DroneController.OnFireballFired += FireballFired;
        DroneController.OnGunAmmoAcquired += OnGunAmmoAcquired;
        DroneController.OnFireballAmmoAcquired += OnFireballAmmoAcquired;
        DroneController.WeaponChanged += OnWeaponChanged;
    }

    private void OnWeaponChanged(bool selectedGun)
    {
        gunAmmoUI.gameObject.SetActive(selectedGun);
        fireballAmmoUI.gameObject.SetActive(!selectedGun);
        gunAmmoUIFP.gameObject.SetActive(selectedGun);
        fireballAmmoUIFP.gameObject.SetActive(!selectedGun);
    }

    private void GunFired()
    {
        Debug.Log("GUN FIRED ");
        if (gunAmmo > 0)
        {
            droneAudioSource.clip = turret;
            droneAudioSource.Play();
            gunAmmo -= 1;
        }
        else
        {
            droneAudioSource.clip = emptyGun;
            droneAudioSource.Play();
        }


    }

    private void FireballFired()
    {

        if (fireballAmmo > 0)
        {
            droneAudioSource.clip = turret;
            droneAudioSource.Play();
            fireballAmmo -= 1;
        }
        else
        {
            droneAudioSource.clip = emptyGun;
            droneAudioSource.Play();
        }
    }

    private void OnGunAmmoAcquired()
    {
        gunAmmo += 100;

    }

    private void OnFireballAmmoAcquired()
    {
        fireballAmmo += 5;

    }

    private void Update()
    {
        gunAmmoTextFP.text = "x " + gunAmmo.ToString();
        gunAmmoTextTP.text = "x " + gunAmmo.ToString();
        fireballAmmoFP.text = "x " + fireballAmmo.ToString();
        fireballAmmoTP.text = "x " + fireballAmmo.ToString();
    }




}