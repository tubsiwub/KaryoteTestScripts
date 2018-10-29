using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public class ApplyMovementNodes : C_Monobehaviour 
	{
		[SerializeField] float distanceBetweenNodes = 2;
		Renderer rend;

		void Awake () 
		{
			rend = GetComponent<Renderer> ();
			StoreNodePoints();
		}

		void StoreNodePoints()
		{
			for (float x = rend.bounds.min.x; x < rend.bounds.max.x; x += distanceBetweenNodes)
			{
				for (float z = rend.bounds.min.z; z < rend.bounds.max.x; z += distanceBetweenNodes)
				{
					NodesContainer.instance.nodePositions.Add (new Vector3 (x, rend.bounds.max.y, z));
				}
			}
		}
	}
}
