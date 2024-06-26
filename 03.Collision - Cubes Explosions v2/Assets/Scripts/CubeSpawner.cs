using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Cube _cube;
    [Header("Terrain")]
    [SerializeField] private Terrain _terrain;

    private const float MultipleScaleOnEachSplit = 0.5f;
    private const float MultipleChanceOnEachSplit = 0.5f;

    private const int SplitCubeMin = 2;
    private const int SplitCubeMax = 6;

    private const int InitialCubeCount = 8;
    private const float InitialScale = 1f;

    private const float MinHeight = 5f;
    private const float MaxHeight = 10f;

    private void Start()
    {
        GenerateCubesOnTerrain(_cube, InitialCubeCount);
    }

    public Cube[] Split(Cube cube)
    {
        float splitChance = cube.SplitChance * MultipleChanceOnEachSplit;
        float scale = cube.ScaleMultiplier * MultipleScaleOnEachSplit;
        int newCubeCount = Random.Range(SplitCubeMin, SplitCubeMax);

        List<Cube> cubes = new List<Cube>();

        for (int i = 0; i < newCubeCount; i++)
        {
            Vector3 spawnPos = cube.transform.position + GetRandomOffset();
            Cube spawnedCube = Spawn(cube, spawnPos, scale, splitChance);
            cubes.Add(spawnedCube);
        }

        return cubes.ToArray();
    }

    private void GenerateCubesOnTerrain(Cube cube, int count)
    {
        float terrainWidth = _terrain.terrainData.size.x;
        float terrainLength = _terrain.terrainData.size.z;

        float xTerrainPos = _terrain.transform.position.x;
        float yTerrainPos = _terrain.transform.position.y;
        float zTerrainPos = _terrain.transform.position.z;

        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
            float randomY = Random.Range(yTerrainPos + MinHeight, yTerrainPos + MaxHeight);
            float randomZ = Random.Range(zTerrainPos, zTerrainPos + terrainLength);

            Spawn(cube, new Vector3(randomX, randomY, randomZ), InitialScale);
        }
    }

    private Vector3 GetRandomOffset(float radius = 1f)
    {
        float offsetX = Random.Range(-1f, 1f) * radius;
        float offsetY = Random.Range(0f, 1f) * radius;
        float offsetZ = Random.Range(-1f, 1f) * radius;

        return new Vector3(offsetX, offsetY, offsetZ);
    }

    private Cube Spawn(Cube cubePrefab, Vector3 position, float scale, float splitChance = 1f)
    {
        Cube cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.transform.SetParent(gameObject.transform, false);
        cube.Init(splitChance, scale);

        return cube;
    }
}
