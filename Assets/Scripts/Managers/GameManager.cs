using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform[] enemySpawnPoints, gunAmmoSpawnPoints, FireBallAmmoSpawnPoints, healthPointSpawnPoints;
        [SerializeField] private GameObject enemyPrefab, healthPrefab, gunAmmoPrefab, fireballAmmoPrefab, WaitForNextWaveCanvas;
        private List<GameObject> enemies = new List<GameObject>();

        public int enemiesKilled = 0;

        private void Start()
        {
            foreach (Transform spawnPoint in enemySpawnPoints)
            {
                var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemies.Add(enemy);
            }
            foreach (Transform spawnPoint in gunAmmoSpawnPoints)
            {
                Instantiate(gunAmmoPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            foreach (Transform spawnPoint in FireBallAmmoSpawnPoints)
            {
                Instantiate(fireballAmmoPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            foreach (Transform spawnPoint in healthPointSpawnPoints)
            {
                Instantiate(healthPrefab, spawnPoint.position, spawnPoint.rotation);

            }
        }

        private void Update()
        {
            CheckDeadEnemies();
        }

        IEnumerator CheckDeadEnemies()
        {
            WaitForNextWaveCanvas.SetActive(true);
            yield return new WaitForSeconds(1);
            WaitForNextWaveCanvas.SetActive(false);
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().RestartEnemy();
            }

        }
    }
}

