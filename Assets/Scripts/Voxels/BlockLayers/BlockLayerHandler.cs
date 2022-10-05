using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    [System.Serializable]
    public abstract class BlockLayerHandler : MonoBehaviour
    {
        [SerializeField] private BlockLayerHandler Next;
        // TODO: Lucas, look into refactoring this to a better chain-of-command methodology.

        public bool Handle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
        {
            if (TryHandling(chunkData, pos, surfaceHeightNoise, mapSeedOffset))
                return true;
            if (Next != null)
                return Next.Handle(chunkData, pos, surfaceHeightNoise, mapSeedOffset);
            return false;
        }

        public abstract bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset);
    }
}

