using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Allele
{
	[System.Serializable]
	public class AnimalStateAnnoyed : IAnimalState 
	{
		string name = "Annoyed";
		public string Name() {return name;}

		public void Behavior(WildAnimal wa)
		{
			// ...
			// Move away until far enough away from the player while staying within wander radius at half speed.
			// If moving away requires leaving wander radius, place marker somewhere else within wanderRadius and double normal speed.
			// We cover the effects of becoming annoyed in DetectStateChange()


			// Get where the player is and avoid it
			wa.CheckPlayerFrontPosition ();


			// If we hit the bounds of the wander area, perform new code (shown below in BoundaryOperation)
			if (wa.reachedBoundary)
				return;

			Vector3 directionFromPlayer = wa.transform.position - PlayerController.instance.transform.position;
			directionFromPlayer.y = 0;
			directionFromPlayer = directionFromPlayer.normalized;
			Vector3 newMarkerPosition = wa.transform.position + (directionFromPlayer * wa.comfortRadius);
			wa.moveToMarker.position = newMarkerPosition;

			// When ANIMAL leaves wander radius
			if (Vector3.Distance (newMarkerPosition, wa.wanderMarker.position) >= wa.wanderRadius) // animal needs a trigger event for when it leaves it's own wander radius ?
			{
				Vector3 newDirection = wa.DetermineReflectionDirection (30.0f);
				newMarkerPosition = wa.transform.position + (newDirection * wa.comfortRadius);
				wa.StartCoroutine (wa.BoundaryOperation (1.0f, newDirection));
				return;
			}

			//if (wa.annoyedTimeoutCounter > 0)
			//	wa.moveToMarker.transform.position = newMarkerPosition;
			//else
			//{
			//	if (wa.annoyedRandomShiftTimer < 0)
			//	{
			//		wa.MoveTargetWildPosition ();
			//		wa.annoyedRandomShiftTimer = wa.annoyedRandomShiftTimerDefault;
			//		wa.annoyedTimeoutCounter = wa.annoyedTimeoutCounterDefault;
			//	}
			//	wa.annoyedRandomShiftTimer -= Time.deltaTime;
			//
			//	wa.moveToMarker.transform.position = (Vector3)AstarPath.active.GetNearest (wa.moveToMarker.transform.position, NNConstraint.Default).node.position;
			//}
		}

		public void Action(WildAnimal wa)
		{

		}

		public void InitializeValues(WildAnimal wa)
		{

		}

		public void ReactionToPlayer(WildAnimal wa)
		{
			wa.annoyedTimeoutCounter -= Time.deltaTime;		
		}

		public void CheckPlayerState(WildAnimal wa)
		{

		}

		public void CheckPlayerPosition(WildAnimal wa)
		{

		}
	}
}