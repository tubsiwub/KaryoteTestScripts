using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Allele
{
	[System.Serializable]
	public class AnimalStateWander : IAnimalState 
	{
		string name = "Wander";
		public string Name() {return name;}

		public void Behavior(WildAnimal wa)
		{
			if (!wa.moveToMarker)
				return;

			if (wa.playerDistance < wa.wanderRadius)
			{
				// Make sure path is valid
				GraphNode n1, n2;
				n1 = AstarPath.active.GetNearest (wa.transform.position).node;
				n2 = AstarPath.active.GetNearest (wa.moveToMarker.transform.position).node;
				if (!PathUtilities.IsPathPossible (n1, n2))
				{
					wa.MoveTargetPosition ();
					return;
				}
			}

			if (Vector3.Distance (wa.transform.position, wa.moveToMarker.position) < wa.stopDistance*2)
				wa.targetReached = true;
			else wa.targetReached = false;

			if (wa.targetReached)
			{
				MoveTargetPositionCheck (wa);
			}

			// If moveToMarker is outside of working radius, ignore rest of movement code
			if (Vector3.Distance (wa.moveToMarker.position, wa.wanderMarker.position) > wa.wanderRadius)
			{
				//MoveTargetPosition ();
				wa.moveToMarker.position = wa.wanderMarker.position;
			}

			if (Vector3.Angle (PlayerController.instance.transform.position - wa.transform.position, wa.moveToMarker.position - wa.transform.position) < 90)
			{
				Vector3 dirToPlayer = (wa.moveToMarker.position - PlayerController.instance.transform.position).normalized;
				wa.moveToMarker.position += dirToPlayer * wa.maxSpeed * Time.deltaTime;
			}

			if (wa.playerDistance < wa.wanderRadius)
			{
				if (AstarPath.active.GetNearest (wa.moveToMarker.transform.position, NNConstraint.Default).node != null)
				{
					wa.moveToMarker.transform.position = (Vector3)AstarPath.active.GetNearest (wa.moveToMarker.transform.position, NNConstraint.Default).node.position;
				}
			}
		}

		public void Action(WildAnimal wa)
		{

		}

		public void InitializeValues(WildAnimal wa)
		{

		}

		public void ReactionToPlayer(WildAnimal wa)	// second update
		{
			if(wa.annoyedTimeoutCounter < 5)
				wa.annoyedTimeoutCounter += Time.deltaTime;

			wa.GetComponent<Renderer> ().material.color = Color.white;

			wa.annoyedTimeoutCounter = wa.annoyedTimeoutCounter < -5 ? -5 : wa.annoyedTimeoutCounter;
		}

		public void CheckPlayerState(WildAnimal wa)
		{

		}

		public void CheckPlayerPosition(WildAnimal wa)
		{

		}

		#region Specific Functions

		public void MoveTargetPositionCheck(WildAnimal wa)
		{
			wa.wanderChangeTimer -= Time.deltaTime;
			if (wa.wanderChangeTimer <= 0)
			{
				wa.MoveTargetPosition ();
			}
		}

		#endregion
	}
}