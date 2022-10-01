using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    [CreateAssetMenu(fileName = "Block Data", menuName = "SOs/Block Data")]
    public class SOBlockData : ScriptableObject
    {
        public float textureSizeX, textureSizeY;
        public List<TextureData> textureDatas;
    }

    [Serializable]
    public class TextureData
    {
        public BlockType blockType;
        public Vector2Int up, down, side;
        public bool isSolid = true;
        public bool generatesCollider = true;
    }
}