using System;
using System.Collections;
using System.Collections.Generic;
using JazzApps;
using UnityEngine;

public class BlockLayers : MonoBehaviour
{
    private AirLayerHandler airLH;
    private WaterLayerHandler waterLH;
    private SurfaceLayerHandler surfaceLH;
    private UndergroundLayerHandler undergroundLH;

    public void Initialize()
    {
        // The first preset
        airLH.Next = waterLH;
        waterLH.Next = surfaceLH;
        surfaceLH.Next = undergroundLH;
        undergroundLH.Next = null;
    }

    private void Awake()
    {
        Initialize();
    }
}
public class AirLayerHandler : BlockLayerHandler
{
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (pos.y > surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, pos, BlockType.AIR);
            return true;
        }
        return false;
    }
}
public class WaterLayerHandler : BlockLayerHandler
{
    public int waterLevel;
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (pos.y > surfaceHeightNoise & pos.y <= waterLevel)
        {
            Chunk.SetBlock(chunkData, pos, BlockType.WATER);
            if (pos.y == surfaceHeightNoise + 1)
            {
                pos.y = surfaceHeightNoise;
                Chunk.SetBlock(chunkData, pos, BlockType.SAND);
            }
            return true;
        }
        return false;
    }
}
public class SurfaceLayerHandler : BlockLayerHandler
{
    public BlockType surfaceblockType;
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (pos.y == surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, pos, surfaceblockType);
            return true;
        }
        return false;
    }
}
public class UndergroundLayerHandler : BlockLayerHandler
{
    public BlockType undergroundblockType;
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (pos.y < surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, pos, undergroundblockType);
            return true;
        }
        return false;
    }
}