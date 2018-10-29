using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	[System.Serializable]
	public class AnimalStateSimple : IAnimalState 
	{
		string name = "Simple";
		public string Name() {return name;}

		float actionCounter = 5.0f;


		public string DisplayName()
		{
			return name;
		}

		// Move at intervals - used instead of normal AI behavior when too far from the player
		public void Behavior(WildAnimal wa)
		{
			actionCounter -= Time.deltaTime;
			if (actionCounter < 0)
			{
				actionCounter = 5.0f;
				wa.MoveTargetPosition ();
			}

			Vector3 moveDirection = wa.moveToMarker.position - wa.transform.position;
			moveDirection = moveDirection.normalized;
			moveDirection.y = 0;

			float simpleSpeed = 4.0f;

			wa.CController.SimpleMove (moveDirection * simpleSpeed);
		}

		public void Action(WildAnimal wa)
		{
			// Purposefully left blank
		}

		public void InitializeValues(WildAnimal wa)
		{
			// Purposefully left blank
		}

		public void ReactionToPlayer(WildAnimal wa)
		{
			// Purposefully left blank
		}

		public void CheckPlayerState(WildAnimal wa)
		{
			// Purposefully left blank
		}

		public void CheckPlayerPosition(WildAnimal wa)
		{
			// Purposefully left blank
		}
	}
}