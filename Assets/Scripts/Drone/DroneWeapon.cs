using UnityEngine;

public class DroneWeapon : MonoBehaviour {
    public int gunAmmo = 100;

    private void Start() {
        DroneController.OnGunFired += GunFired;
    }

    private void GunFired(){
        gunAmmo -= 1;
    }

    

} 