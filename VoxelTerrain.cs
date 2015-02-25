using UnityEngine;
using System.Collections.Generic;

public class VoxelTerrain : MonoBehaviour
{
	public int gridSize = 50;
	public float height;
	public float snowLevel;
	public Color snowColor;
	public float rockLevel;
	public Color rockColor;
	public Color grassColor;
	public float sandLevel;
	public Color sandColor;
	public float seaLevel;
	public Dictionary<int,Tile> tiles = new Dictionary<int,Tile>();

	public Mesh partMesh;
	public Vector3[] Vertices;
	public Vector2[] UV;
	public Color[] colors;
	public int[] triangles;

	public Material partMaterial;
	public Material waterMaterial;
	private Water water;

	public void Start()
	{
		//TODO generate a random heightmap
		Vertices = new Vector3[gridSize*gridSize];
		UV = new Vector2[Vertices.Length];
		colors = new Color[Vertices.Length];
		triangles = new int[Vertices.Length*9];
		int seed = Random.Range (0, 100);

		for(int y = 0; y < gridSize; y++)
		{
			for(int x = 0; x < gridSize; x++)
			{
				float newY = 0;
				if(x == 0 || x == gridSize-1 || y == 0 || y == gridSize-1) newY = 0;
				else
				{
					newY = Noise(x+seed,y+seed,10,3,2);
					newY += Noise(x+seed,y+seed,2,1,1);
					newY += Random.Range(-0.5f,0.5f);
					if(x < gridSize/2) newY *= (float)x/(float)gridSize;
					else newY *= 1-(float)x/(float)gridSize;
					if(y < gridSize/2) newY *= (float)y/(float)gridSize;
					else newY *= 1- (float)y/(float)gridSize;
					newY *= height;
				}
				if(newY > snowLevel) colors[y*gridSize + x] = snowColor;
				else if(newY > rockLevel) colors[y*gridSize + x] = rockColor;
				else if(newY < sandLevel) colors[y*gridSize + x] = sandColor;
				else colors[y*gridSize + x] = grassColor;

				Vertices[y*gridSize+ x] = new Vector3(x,newY,y);
				UV[y*gridSize+ x] = new Vector2((float)x/(float)(gridSize-1),(float)y/(float)(gridSize-1));
			}
		}

		for(int y = 0; y < gridSize-1; y++)
		{
			for(int x = 0; x < gridSize-1; x++)
			{
				//create the triangles
				triangles[6*(y*gridSize+x)] = y*gridSize+x;
				triangles[6*(y*gridSize+x)+1] = y*gridSize +x+ gridSize;
				triangles[6*(y*gridSize+x)+2] = y*gridSize+x +1;
				triangles[6*(y*gridSize+x)+3] = y*gridSize+x +1 + gridSize;
				triangles[6*(y*gridSize+x)+4] = y*gridSize+x +1;
				triangles[6*(y*gridSize+x)+5] = y*gridSize+x+gridSize;
			}
		}
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		gameObject.renderer.material = partMaterial;

		partMesh = new Mesh ();
		partMesh.vertices = Vertices;
		partMesh.colors = colors;
		partMesh.uv = UV;
		partMesh.triangles = triangles;
		partMesh = MeshHelper.FlatShading (partMesh, 1);
		
		gameObject.transform.GetComponent<MeshFilter> ().mesh = partMesh;
		MeshCollider collider = gameObject.AddComponent<MeshCollider> ();

		transform.position = new Vector3(-gridSize/2,0,-gridSize/2);

		//Create water
		GameObject Water = new GameObject ("Water");
		Water.transform.parent = transform;
		water = Water.AddComponent<Water>();
		water.partMaterial = waterMaterial;
		Water.AddComponent<MeshFilter> ();
		Water.AddComponent<MeshRenderer> ();
		water.speed = 1;
		water.waveHeight = 1;
		water.Init (seaLevel,gridSize);

		GameObject DeepWater = new GameObject ("DeepWater");
		DeepWater.transform.parent = transform;
		water = DeepWater.AddComponent<Water>();
		water.partMaterial = waterMaterial;
		DeepWater.AddComponent<MeshFilter> ();
		DeepWater.AddComponent<MeshRenderer> ();
		water.speed = 1;
		water.waveHeight = 1;
		water.Init (seaLevel-0.25f,gridSize);
	}
	float Noise (int x, int y, float scale, float mag, float exp)
	{	
		return (Mathf.Pow (Mathf.PerlinNoise(x/scale,y/scale)*mag,exp)); 
	}
}