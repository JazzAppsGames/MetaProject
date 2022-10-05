using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JazzApps
{
    /// <summary>
    /// TODO: make a summary lol
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        // Externals
        public SOMapGeneration SOMapGeneration;
        
        // Internals
        private List<GameObject> mapGOPool;

        private void Awake()
        {
            GenerateMap();
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
                GenerateMap();
        }

        public void GenerateMap()
        {
            ClearMap();

            // TODO: Generate with perlin noise
            // HACK: the next lines of code are temporary, it just generates a weird border using the data.
            MapGenerationData data = SOMapGeneration.mapGenData;
            short seed = data.seed == 0 ? (short)Random.Range(short.MinValue, short.MaxValue) : data.seed;
            mapGOPool.Add(Instantiate(SOMapGeneration.mapGenPrefabs[0], Vector3.zero, Quaternion.identity));
            int mapRadius = (int)data.mapRadius;
            for (int i = -mapRadius+1; i < mapRadius; i++)
            {
                mapGOPool.Add(Instantiate(SOMapGeneration.mapGenPrefabs[0], new Vector3(-mapRadius, Random.Range(-data.maxTerrainBottom, data.maxTerrainHeight), i), Quaternion.identity));
                mapGOPool.Add(Instantiate(SOMapGeneration.mapGenPrefabs[0], new Vector3(mapRadius, Random.Range(-data.maxTerrainBottom, data.maxTerrainHeight), i), Quaternion.identity));
                mapGOPool.Add(Instantiate(SOMapGeneration.mapGenPrefabs[0], new Vector3(i, Random.Range(-data.maxTerrainBottom, data.maxTerrainHeight), -mapRadius), Quaternion.identity));
                mapGOPool.Add(Instantiate(SOMapGeneration.mapGenPrefabs[0], new Vector3(i, Random.Range(-data.maxTerrainBottom, data.maxTerrainHeight), mapRadius), Quaternion.identity));
            }
        }

        private void ClearMap()
        {
            // TODO: Actually pool the objects lmao
            try
            {
                foreach (var GO in mapGOPool)
                {
                    Destroy(GO);
                }
            }
            catch (Exception e) { }
            
            mapGOPool ??= new List<GameObject>();
        }
    }
}