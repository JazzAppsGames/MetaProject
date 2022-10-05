using System.Collections;
using System.Collections.Generic;
using JazzApps;
using UnityEngine;

namespace JazzApps
{
    public class NoiseGenerator : MonoBehaviour
    {
        public static float RemapValue(float value, float initialMin, float initialMax, float outputMin, float outputMax) =>
            outputMin + (value - initialMin) * (outputMax - outputMin) / (initialMax - initialMin);
        
        public static float RemapValue01(float value, float outputMin, float outputMax) =>
            outputMin + (value - 0) * (outputMax - outputMin) / (1 - 0);
        public static int RemapValue01ToInt(float value, float outputMin, float outputMax) =>
            (int)RemapValue01(value, outputMin, outputMax);

        public static float Redistribution(float noise, NoiseConfiguration config) =>
            Mathf.Pow(noise * config.redistributionModifier, config.exponent);

        public static float OctavePerlin(float x, float z, NoiseConfiguration config)
        {
            x *= config.noiseZoom;
            z *= config.noiseZoom;
            x += config.noiseZoom;
            z += config.noiseZoom;

            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            float amplitudeSum = 0;  // Used for normalizing result to 0.0 - 1.0 range
            for (int i = 0; i < config.octaves; i++)
            {
                total += Mathf.PerlinNoise((config.offset.x + config.worldOffset.x + x) * frequency, (config.offset.y + config.worldOffset.y + z) * frequency) * amplitude;

                amplitudeSum += amplitude;

                amplitude *= config.persistence;
                frequency *= 2;
            }

            return total / amplitudeSum;
        }
    }
}