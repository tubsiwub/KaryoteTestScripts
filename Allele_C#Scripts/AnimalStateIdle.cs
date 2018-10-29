using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Allele
{
	[System.Serializable]
	public class AnimalStateIdle : IAnimalState 
	{
		string name = "Idle";
		public string Name() {return name;}

		public void Behavior(WildAnimal wa)
		{

		}

		public void Action(WildAnimal wa)
		{

		}

		public void InitializeValues(WildAnimal wa)
		{
			
		}

		public void ReactionToPlayer(WildAnimal wa)
		{
			if (wa.playerRadiusPosition == 3)	// within comfort layer
			{
				wa.GetComponent<Renderer> ().material.color = new Color (1.0f, 0.45f, 0.0f, 1.0f);	// orange?

				wa.animalState = wa.animalStates["annoyed"];
				wa.annoyedTimeoutCounter -= Time.deltaTime;

				wa.CheckForBaitTotalLockout ();

				// If the player isn't holding down Offer Bait, become annoyed
				if (!PlayerController.instance.CheckPlayerState("offerbait"))
				{
					wa.animalState = wa.animalStates["annoyed"];
					wa.EndBaitProcess ();
				}
			}
			else
			{
				if(wa.annoyedTimeoutCounter < 5)
					wa.annoyedTimeoutCounter += Time.deltaTime;
			}

			if (wa.playerRadiusPosition == 2)	// within action layer (no comfort)
			{
				wa.GetComponent<Renderer> ().material.color = Color.white;

				wa.annoyedTimeoutCounter = wa.annoyedTimeoutCounter < -5 ? -5 : wa.annoyedTimeoutCounter;
			}

			if (wa.playerRadiusPosition == 1)	// within wander layer (no action, comfort)
			{
				wa.GetComponent<Renderer> ().material.color = Color.blue;
			}
		}

		public void CheckPlayerState(WildAnimal wa)
		{

		}

		public void CheckPlayerPosition(WildAnimal wa)
		{

		}
	}
}