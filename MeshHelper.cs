using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshHelper
{
	public static Mesh FlatShading(Mesh baseMesh, int subMeshes)
	{
		Mesh unsharedVertexMesh = new Mesh();
		
		for(int n = 0; n < subMeshes; n++)
		{
			int[] sourceIndices = baseMesh.GetTriangles(n);
			Vector3[] sourceVerts = baseMesh.vertices;
			Vector2[] sourceUVs = baseMesh.uv;
			Color[] sourceColors = baseMesh.colors;
			
			int[] newIndices = new int[sourceIndices.Length];
			Vector3[] newVertices = new Vector3[sourceIndices.Length];
			Vector2[] newUVs = new Vector2[sourceIndices.Length];
			Color[] newColors = new Color[sourceIndices.Length];
			// Create a unique vertex for every index in the original Mesh:
			for(int i = 0; i < sourceIndices.Length; i++)
			{
				newIndices[i] = i;
				newVertices[i] = sourceVerts[sourceIndices[i]];
				if(sourceUVs.Length > 0)
				{
					newUVs[i] = sourceUVs[sourceIndices[i]];
				}
				if(sourceColors.Length > 0)
				{
					newColors[i] = sourceColors[sourceIndices[i]];
				}
			}
			unsharedVertexMesh.vertices = newVertices;
			unsharedVertexMesh.uv = newUVs;
			unsharedVertexMesh.colors = newColors;
			unsharedVertexMesh.SetTriangles (newIndices,n);
		}
		
		unsharedVertexMesh.RecalculateNormals ();
		
		return unsharedVertexMesh;
	}
}