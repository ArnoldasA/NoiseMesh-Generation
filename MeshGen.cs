using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//inspired off https://www.youtube.com/watch?v=4RpVBYW1r5M&t=832s&ab_channel=SebastianLague
[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]

public abstract class MeshGen : MonoBehaviour 
{
	[SerializeField] protected Material material;

	protected List<Vector3> vertices;
	protected List<int> triangles;
	protected List<Vector3> normals;
	protected int numVertices;
	protected int numTriangles;
	private MeshFilter meshFilter;
	protected MeshRenderer meshRenderer;
	private MeshCollider meshCollider;
	private Mesh mesh;


	private void Declrations()
	{
		vertices = new List<Vector3>();
		triangles = new List<int>();

		//optional
		normals = new List<Vector3>();
	}
	public  void Generate()
    {
		//Gen Terrain
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		Declrations();
		SetMeshNums();
		meshRenderer.material = material;
		CreateMesh();
	}

	private void CreateMesh()
	{
		mesh = new Mesh ();
		SetVertices ();
		SetTriangles ();
		SetNormals ();	
			mesh.SetVertices (vertices);
			mesh.SetTriangles (triangles, 0);

			if (normals.Count == 0)
			{
				mesh.RecalculateNormals ();
				normals.AddRange (mesh.normals);
			}
			mesh.SetNormals (normals);
			meshFilter.mesh = mesh;
			meshCollider.sharedMesh = mesh;	
	}

	//Go through triangle and add up normals on each vertex to get total
	//Normal same at each corner
	//add normalized total
	protected void SetGeneralNormals()
	{
		int Triangles = numTriangles / 3;
		Vector3[] norms = new Vector3[numVertices];
		int index = 0;
		for (int i=0; i<Triangles; i++)
		{
			//the triangle ints that make up a geometric triangle 
			int triA = triangles [index];
			int triB = triangles [index + 1];
			int triC = triangles [index + 2];

			//directions that make up the triangle
			Vector3 dirA = vertices[triB] - vertices[triA];
			Vector3 dirB = vertices[triC] - vertices[triA];

			//Get normal out of plane
			Vector3 normal = Vector3.Cross(dirA, dirB);

			//add the normals 
			norms[triA] += normal;
			norms[triB] += normal;
			norms[triC] += normal;

			index += 3;
		}

		//go through the vertices and normalise 
		for (int i=0; i<numVertices; i++)
		{
			normals.Add (norms [i].normalized);
		}
	}
	protected abstract void SetVertices();
	protected abstract void SetTriangles();

	protected abstract void SetNormals();
	protected abstract void SetMeshNums();
}
