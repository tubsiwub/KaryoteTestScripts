using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Allele
{
	[System.Serializable]
	public class AnimalStateBait : IAnimalState 
	{
		string name = "Bait";
		public string Name() {return name;}



		// Bait-specific variables
		[Tooltip("This + baitMoveInterval + (10*animalAffinity), so anything above 10 for this might be overkill")]
		public float chanceForBaitSuccess;

		public bool waitForAnimalMove = false;
		public Vector3 animalStartPosition;

		public float distanceBeforeAcceptBait;	// chance of taking bait with no success as well as chance of success determined when this gets to near-zero;
		public float actionRadiusSizeChangeAmount;		// amount of shrinkage when bait attempt has some success
		public float comfortRadiusSizeChangeAmount;		// ... /\ /\ /\


		public void Behavior(WildAnimal wa)
		{
			if (!PlayerController.instance.CheckPlayerState("offerbait") || wa.waitForPlayerNextAction)
				return;

			// Check how close the animal is to it's current destination, then mark that as a bool
			float animalDistanceToMovePoint = Vector3.Distance (wa.moveToMarker.transform.position, MathFunctions.ShiftToCenterPoint (animalStartPosition, wa.baitHolderTarget.position, wa.percentageDistance));
			if (animalDistanceToMovePoint > 2)
				waitForAnimalMove = true;
			else
				waitForAnimalMove = false;

			wa.moveToMarker.transform.position = MathFunctions.ShiftToCenterPoint (animalStartPosition, wa.baitHolderTarget.position, wa.percentageDistance);

			if (waitForAnimalMove)	// let animal reach destination first
				return;

			// While being baited, count down toward a final action
			if (wa.baitTimeoutCounter > 0)
			{
				wa.baitTimeoutCounter -= Time.deltaTime;

				wa.GetComponent<Renderer> ().material.color = Color.red;
			}
			else
			{
				// Increase pDist which determines where the animal rests in relation to the player
				wa.percentageDistance += wa.baitMoveInterval;

				// Shrink comfort/action radius' so the player is forced to move closer - shrinks based on original distances
				float shrinkAmount = Vector3.Distance(PlayerController.instance.transform.position, wa.transform.position);
				wa.actionRadius = shrinkAmount + 1;
				wa.comfortRadius = shrinkAmount / 2 + 1;

				// Reset timer
				wa.baitTimeoutCounter = wa.baitTimeoutCounterDefault;

				// Pause animal actions until the player reinitiates the bait process
				wa.waitForPlayerNextAction = true;

				wa.GetComponent<Renderer> ().material.color = new Color (1.0f, 0.92f, 0.016f, 1.0f);	//yellow
			}

			float animalDistance = Vector3.Distance (PlayerController.instance.transform.position, wa.transform.position);
			if (wa.percentageDistance >= 80 && animalDistance < 3)
			{
				// Check for success / failure when taking bait here
				float  chance = Random.Range (0, 100);
				if (chance < wa.chanceForTotalLockout)
				{
					Debug.Log ("TOTAL LOCKOUT " + chance + " < " + wa.chanceForTotalLockout + " = true");
					GameObject.Destroy (wa.transform.gameObject);

					Inventory.instance.RemoveBait (Inventory.instance.currentBait.ToString (), 1);
				}
				else
				{
					chance = Random.Range(0,100);	// re-roll to make values < totalLockout valid
					if (chance < chanceForBaitSuccess + (wa.animalAffinity * 10) + wa.baitMoveInterval)	// up to a max of 100% currently (50% +0, +10, +20, +30, +40 -> 100% with +50) + baitMoveInterval to make better plays more rewarded (20, 27, 40)
					{
						// Here is where we lock the animal from all action, play the motion of it running to the player
						//...

						Debug.Log ("SUCCESS!  Here is where we display the menu for the player to decide on an action to take.");
						GameObject.Destroy (wa.transform.gameObject);

						Inventory.instance.RemoveBait (Inventory.instance.currentBait.ToString (), 1);
					}
					else
					{
						// Here is where we lock the animal from all action, play the motion of it running to the player, nabbing the bait and running away
						//...

						wa.recentlyBaited = true;
						Debug.Log ("Unsuccessful (" + wa.animalAffinity + " : " + (chanceForBaitSuccess + (wa.animalAffinity * 10) + wa.baitMoveInterval) + ")  Animal took bait and likes the player a bit more now.");
						wa.animalAffinity += wa.animalAffinity > 5 ? 0 : 1;	// 5 max?

						Inventory.instance.RemoveBait (Inventory.instance.currentBait.ToString (), 1);
					}
				}

				// end bait, restore values here
				wa.EndBaitProcess();
			}

			// Rules:
			// Notice and walk toward player a bit before stopping - distance based on player's start distance
			// Shrink comfort zone allowing the animal to come closer without fear
			// Check if the comfort zone is smaller than a certain radius, if so we animate the animal to come to the player's hand
			// If the player at any point in time stops holding the Bait button, animal resets, but not the ChanceForTotalLockout or comfort radius, which increases when comfort zone is entered
			// Entering comfort layer resets comfort layer, checks for total lockout, and if total lockout fails, increases the chance for total lockout if bait was offered recently
		}

		public void Action(WildAnimal wa)
		{
			
		}

		public void InitializeValues(WildAnimal wa)
		{
			// False since we're starting a new bait cycle
			wa.recentlyBaited = false;

			if (!CheckIfEnoughBait (wa))
			{
				wa.animalState = wa.animalStates["wander"];
				return;
			}

			// Check which percentage the player is within the action radius (from wander to comfort)
			// Set the BaitMoveInterval to either 20.25, 27 or 40.5 based on distance correlating to how many attempts it will take to fully tame the animal
			{
				// action layer min = comfortRadius, max = actionRadius
				float percentageWithinActionLayer = MathFunctions.ConvertNumberRanges(wa.playerDistance, wa.actionRadius, wa.comfortRadius, 100, 0);	// out of 100%

				if (percentageWithinActionLayer > 70)
					wa.baitMoveInterval = 20.25f;
				else if (percentageWithinActionLayer > 40)
					wa.baitMoveInterval = 27.0f;
				else if (percentageWithinActionLayer > 0)
					wa.baitMoveInterval = 40.5f;
			}

			// Animal's current position is now the starting point
			animalStartPosition = wa.transform.position;
		}

		public void ReactionToPlayer(WildAnimal wa)
		{
			if (wa.playerRadiusPosition == 3)	// within comfort layer
			{
				// If the player isn't holding down Offer Bait, become annoyed
				if (!PlayerController.instance.CheckPlayerState("offerbait"))
				{
					wa.animalState = wa.animalStates["annoyed"];
					wa.EndBaitProcess ();
				}
			}

			if (wa.playerRadiusPosition == 1)	// within wander layer (no action, comfort)
			{
				// lose interest if player exits action layer
				if(!PlayerController.instance.CheckPlayerState("offerbait"))
				{
					wa.animalState = wa.animalStates["wander"];
					wa.EndBaitProcess ();
				}
			}
		}

		public void CheckPlayerState(WildAnimal wa)
		{

		}

		public void CheckPlayerPosition(WildAnimal wa)
		{

		}

		#region Specific Functions

		// We make sure the player has proper bait equipped for the animal, AND that the player has ENOUGH of said bait
		public bool CheckIfEnoughBait(WildAnimal wa)
		{
			if (!Inventory.instance.CheckBaitAmount (Inventory.instance.currentBait.ToString (), 1))
			{
				//print ("Not enough bait in bag.");
				return false;
			}

			bool allowed = false;
			for (int i = 0; i < wa.allowedBait.Length; i++)
			{
				if (Inventory.instance.currentBait.ToString ().ToLower() == wa.allowedBait [i].ToString().ToLower())
					allowed = true;

				//print ("Bait Inv.: " + Inventory.instance.currentBait.ToString () + " : Allowed type: " + allowedBait [i]);
			}

			//print ("Allowed: " + allowed);
			return allowed;
		}

		#endregion
	}
}
