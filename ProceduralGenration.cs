using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class ProceduralGenration : MeshGen 
{
	 private int xResolution = 250;
	 private int zResolution = 250;

	[SerializeField] private float meshScale=1;
	[SerializeField] private float yScale = 1;
	[SerializeField] private List<GameObject> trees;
	[SerializeField, Range(1, 8)] private int octaves = 1;
	[SerializeField] private float lacunarity = 2;
	[SerializeField, Range(0, 1)] private float gain = 0.5f; //needs to be between 0 and 1 so that each octave contributes less to the final shape.
	[SerializeField] private float perlinScale = 1;
	private Vector3 treePosition;
	public GameObject tree;
	private MeshGen mesh;

    private void Start()
    {
		mesh = GetComponent<MeshGen>();
    }


    public void ActuallyGenerate()
    {
		mesh.Generate();
		meshScale = UnityEngine.Random.Range(20, 60);
		octaves= UnityEngine.Random.Range(0, 8);
		yScale= UnityEngine.Random.Range(0f, 2);
		lacunarity= UnityEngine.Random.Range(1, 4);
		perlinScale= UnityEngine.Random.Range(0.6f, 2);
		gain= UnityEngine.Random.Range(0f, 1);
	}

	protected override void SetMeshNums ()
	{
		numVertices = (xResolution + 1) * (zResolution + 1);  //number of vertices in x direction multiplied by number in z
		
	}
	protected override void SetVertices()
	{
		float Vx, y, Vz = 0; //Calculate grid and noise 
		
		
		Noise noise = new Noise(octaves, lacunarity, gain, perlinScale);
        foreach (var tree in trees)
        {
			Destroy(tree);
        }
		trees.Clear();
		for (int z = 0; z <= zResolution; z++)
		{
			for (int x = 0; x <= xResolution; x++)
			{

				Vx = ((float)x / xResolution) * meshScale;
				Vz = ((float)z / zResolution) * meshScale;

				y = yScale * noise.FractalNoise(Vx, Vz);

				vertices.Add(treePosition = new Vector3(Vx, y, Vz));
			
					if (UnityEngine.Random.Range(0, Mathf.Sqrt(meshScale)*xResolution) < meshScale/10)
					{
						

						GameObject Clone = Instantiate(tree, treePosition, Quaternion.identity);
						trees.Add(Clone);

					}
				}
			}
		}
	
	protected override void SetTriangles ()
	{
		int count = 0;
		for (int z = 0; z < zResolution; z++) 
		{
			for (int x = 0; x < xResolution; x++) 
			{
				triangles.Add (count);
				triangles.Add (count + xResolution + 1);
				triangles.Add (count + 1);

				triangles.Add (count + 1);
				triangles.Add (count + xResolution + 1);
				triangles.Add (count + xResolution + 2);

				count++;
			}
			count++;
		}
	}

    protected override void SetNormals ()	{	}
	
	
}
