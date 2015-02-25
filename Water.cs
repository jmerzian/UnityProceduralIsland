using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
	public Mesh partMesh;
	public Vector3[] Vertices;
	public Vector2[] UV;
	public int[] triangles;
	public int dimensions;

	public Material partMaterial;
	public float speed;
	public float seaLevel;
	public float waveHeight;

	public void Init(float elevation, int gridSize)
	{
		dimensions = gridSize/10 + 1;
		seaLevel = elevation;
		Vertices = new Vector3[dimensions * dimensions];
		triangles = new int[Vertices.Length*9];

		for(int x = 0; x < dimensions;x++)
		{
			for(int y = 0; y < dimensions; y++)
			{
				Vertices[y*dimensions + x] = new Vector3(x*10,elevation,y*10);
			}
		}
		for(int y = 0; y < dimensions-1; y++)
		{
			for(int x = 0; x < dimensions-1; x++)
			{
				triangles[6*(y*dimensions+x)] = y*dimensions+x;
				triangles[6*(y*dimensions+x)+1] = y*dimensions +x+ dimensions;
				triangles[6*(y*dimensions+x)+2] = y*dimensions+x +1;
				triangles[6*(y*dimensions+x)+3] = y*dimensions+x +1 + dimensions;
				triangles[6*(y*dimensions+x)+4] = y*dimensions+x +1;
				triangles[6*(y*dimensions+x)+5] = y*dimensions+x+dimensions;
			}
		}

		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		gameObject.renderer.material = partMaterial;
		
		partMesh = new Mesh ();
		partMesh.vertices = Vertices;
		partMesh.triangles = triangles;
		partMesh.uv = UV;
		partMesh = MeshHelper.FlatShading (partMesh, 1);
		
		gameObject.transform.GetComponent<MeshFilter> ().mesh = partMesh;
		
		transform.position = new Vector3(-gridSize/2,0,-gridSize/2);
	}

	public void Update()
	{	
	//TODO maybe throw an animation in here...
	}
	
	float Noise (int x, int y, float scale, float mag, float exp)
	{	
		return (Mathf.Pow (Mathf.PerlinNoise(x/scale,y/scale)*mag,exp)); 
	}
}

