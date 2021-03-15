using UnityEngine;
using System.Collections;

public class Noise
{
	private int octaves; //Detail 
	private  float lacunarity; //how fast we change the frequency
	private float gain; //amplitude change speed
	private float perlinScale;

	public Noise(){}

	public Noise(int octaves, float lacunarity, float gain, float perlinScale)
	{
		this.octaves = octaves;
		this.lacunarity = lacunarity;
		this.gain = gain;
		this.perlinScale = perlinScale;
	}
	
	public float PerlinNoise(float x, float z) //coordinates 
	{
		//Float between 0-1 to create noise
		return (2 * Mathf.PerlinNoise (x, z) - 1);
	}

	public float FractalNoise(float x, float z)
	{
		float fractalNoise = 0;

		float frequency = 1;
		float amplitude = 1;

		for (int i=0; i<octaves; i++)
		{
			float xVal = x * frequency * perlinScale;
			float zVal = z * frequency * perlinScale;

			fractalNoise += amplitude * PerlinNoise (xVal, zVal);

			frequency *= lacunarity;//frequency change
			amplitude *= gain;// how quickly amplitude changes
		}

		return fractalNoise;
	}
}
