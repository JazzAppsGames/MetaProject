using System;
using System.Collections;
using System.Collections.Generic;
using JazzApps;
using UnityEngine;

public class BlockLayers : MonoBehaviour
{
    // Internals
    private AirLayerHandler airLH = new AirLayerHandler();
    private WaterLayerHandler waterLH = new WaterLayerHandler();
    private SurfaceLayerHandler surfaceLH = new SurfaceLayerHandler();
    private UndergroundLayerHandler undergroundLH = new UndergroundLayerHandler();
    private BlockLayerHandler first;

    public BlockLayerHandler GetStartLayer(SOMapConfiguration config)
    {
        // TODO: Implement changing the block handling via customization
        waterLH.waterLevel = config.waterLevel;
        surfaceLH.surfaceblockType = BlockType.GRASS_DIRT;
        undergroundLH.undergroundblockType = BlockType.DIRT;

        BlockLayerHandler first = waterLH;
        waterLH.Next = airLH;
        airLH.Next = surfaceLH;
        surfaceLH.Next = undergroundLH;
        undergroundLH.Next = null;

        return first;
    }
    
    
}
public class AirLayerHandler : BlockLayerHandler
{
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int groundPosition, Vector2Int mapSeedOffset)
    {
        if (pos.y > groundPosition)
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
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int groundPosition, Vector2Int mapSeedOffset)
    {
        if (pos.y >= groundPosition & pos.y <= waterLevel)
        {
            Chunk.SetBlock(chunkData, pos, BlockType.WATER);
            if (pos.y == groundPosition)
            {
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
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int groundPosition, Vector2Int mapSeedOffset)
    {
        if (pos.y == groundPosition)
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
    public override bool TryHandling(ChunkData chunkData, Vector3Int pos, int groundPosition, Vector2Int mapSeedOffset)
    {
        if (pos.y < groundPosition)
        {
            Chunk.SetBlock(chunkData, pos, undergroundblockType);
            return true;
        }
        return false;
    }
}