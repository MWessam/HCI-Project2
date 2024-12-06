using UnityEngine;


    public class WaveManager : MonoBehaviour
    {
        public GameObject zombiePrefab; 
        public Transform[] spawners;     
        private int[] zombiesPerWave = { 6, 10, 14, 18, 20, 22, 24 };

        private int currentWave = 0;

        public void StartWave()
        {
            if (currentWave < zombiesPerWave.Length)
            {
                SpawnZombies(zombiesPerWave[currentWave]);
                currentWave++;
            }
        }

        private void SpawnZombies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawner = spawners[Random.Range(0, spawners.Length)];
                Instantiate(zombiePrefab, spawner.position, Quaternion.identity);
            }
        }
    }



