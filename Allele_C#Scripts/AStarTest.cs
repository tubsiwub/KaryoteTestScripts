using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public class AStarTest : C_Monobehaviour 
	{
		GameObject[] nodeArr;
		List<Node> nodes;

		struct Node
		{
			public Vector3[] gizmoLine;
		}

		void Start () 
		{
			CalcPaths ();
		}

		void CalcPaths()
		{
			nodes = new List<Node> ();
			nodeArr = GameObject.FindGameObjectsWithTag ("Node");

			for(int i = 0; i < nodeArr.Length; i++)
				for (int j = 0; j < nodeArr.Length; j++)
					if (Vector3.Distance (nodeArr [i].transform.position, nodeArr [j].transform.position) < 3 &&
					    Mathf.Abs(nodeArr [i].transform.position.y - nodeArr [j].transform.position.y) < 1)
					{
						Vector3 direction = nodeArr [j].transform.position - nodeArr [i].transform.position;
						direction = direction.normalized;
						RaycastHit hit;
						if (!Physics.Raycast (nodeArr [i].transform.position, direction, out hit, 3))//, LayerMask.GetMask("AStarLayer")))
						{
							Node newNode = new Node ();
							newNode.gizmoLine = new Vector3[2] { nodeArr [i].transform.position, nodeArr [j].transform.position };
							nodes.Add (newNode);
						}
					}
		}

		void Update () 
		{
			if (Input.GetKeyDown (KeyCode.Space))
			{
				CalcPaths ();
			}
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				return;

			foreach(Node n in nodes)
				Gizmos.DrawLine (n.gizmoLine[0], n.gizmoLine[1]);
		}
	}
}
