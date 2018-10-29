using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Reflection;
using TMPro;

namespace Allele
{
	public class AnimalStorageUI : C_Monobehaviour 
	{
		AnimalStoredInfo storedNPC;

		protected InteractComputer selectedComputer;
		protected AnimalTile currentAnimalTile;

		[Header("Text Fields")]
		public TextMeshProUGUI attackField;
		public TextMeshProUGUI vitalityField;
		public TextMeshProUGUI armorField;
		public TextMeshProUGUI speedField;
		public TextMeshProUGUI enduranceField;
		public TextMeshProUGUI sneakField;

		[Header("Slider Bars")]
		public float statBarMaxWidth = 222f;
		public RectTransform attackSlider;
		public RectTransform vitalitySlider;
		public RectTransform armorSlider;
		public RectTransform speedSlider;
		public RectTransform enduranceSlider;
		public RectTransform sneakSlider;

		[Header("Animal List")]
		[Tooltip("Prefab defining the animal tile (used for showing your stored animals)")]
		public GameObject animalTilePrefab;
		[Tooltip("Container for all animal tiles.")]
		public Transform animalListContent;

		#region MonoBehaviour functions
		public static AnimalStorageUI instance;
		protected void Awake()
		{
			#region Singleton
			if (AnimalStorageUI.instance == null)
				instance = this;
			else if (AnimalStorageUI.instance != this)
				Destroy (this.gameObject);
             			#endregion
		}

		protected void Start()
		{
			InteractComputer[] computers = FindObjectsOfType<InteractComputer> ();
			foreach(InteractComputer comp in computers)
			{
				if (comp.currentlyActive)
					selectedComputer = comp;
			}

			// Spawn in current tiles
			if (AnimalStorage.instance.animalInfoDictionary != null)
			{
				foreach (KeyValuePair<int, AnimalStoredInfo> kvp in AnimalStorage.instance.animalInfoDictionary)
				{
					CreateAnimalTile (kvp.Value, false);
				}
			}
		}

		protected void OnDestroy()
		{
			attackField = null;
			vitalityField = null;
			armorField = null;
			speedField = null;
			enduranceField = null;
			sneakField = null;

			attackSlider = null;
			vitalitySlider = null;
			armorSlider = null;
			speedSlider = null;
			enduranceSlider = null;
			sneakSlider = null;

			selectedComputer = null;
			animalTilePrefab = null;
			animalListContent = null;
		}
		#endregion

		public virtual void SetNameSexValue(string name, string sex)
		{
			// ...
		}

		/// <summary>
		/// Specific to a related UI computer and utilizes a 'switch' to determine what to change.
		/// Switch uses the input string.
		/// </summary>
		/// <param name="stat">[string] Stat, used in Switch.</param>
		/// <param name="modifier">[int] Value, displayed as stat and changes length of visual aids.</param>
		public virtual void SetStatValue(string stat, int modifier)
		{
			switch (stat)
			{
				case "attack":
					attackField.text = modifier + " / 100";
					attackSlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
				case "vitality":
					vitalityField.text = modifier + " / 100";
					vitalitySlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
				case "armor":
					armorField.text = modifier + " / 100";
					armorSlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
				case "speed":
					speedField.text = modifier + " / 100";
					speedSlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
				case "endurance":
					enduranceField.text = modifier + " / 100";
					enduranceSlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
				case "sneak":
					sneakField.text = modifier + " / 100";
					sneakSlider.sizeDelta = new Vector2 (MathFunctions.ConvertNumberRanges (modifier, 100f, 0f, statBarMaxWidth, 0f), 20f);
					break;
			}
		}


		/// <summary>
		/// Stores the current animal into the Creature Storage:
		/// 
		/// Accesses 'Command Animal' script, finds selectedNPC to store.
		/// 
		/// Takes and stores the 'Animal Info' component from the animal.
		/// 
		/// 'Animal Stored Info' contains all necessary animal information,
		///    'Animal Info' contains 'Animal Stored Info' and can be taken 
		///    as an object from the info object.
		/// 
		/// Once stored, the animal MUST be removed from 'Command Animal',
		///     and the best way to do this is to call Deactivate() from
		///     'Recruit Animal', a component attached to the animal AI
		/// 	which allows the player to interact with them.
		/// 
		/// At this point, the animal is stored as data... so we no longer
		/// 	need the animal's gameObject lying around.  Call Destroy().
		/// 
		/// Previously, I had CreateAnimalTile() called separately from outside
		/// 	the script, but we've condensed it as the tile represents the 
		/// 	stored animal and provides the only access to it.  Plus, the
		/// 	animal can only be stored from a menu displaying it.
		/// 
		/// </summary>
		public void StoreAnimalInfo()
		{
			NPCAIPathController npc = CommandAnimal.instance.selectedNPC;
			if (!npc) return;
			AnimalInfo info = npc.GetComponent<AnimalInfo> ();
			if (!info) return;

			storedNPC = info.info;
			storedNPC.icon = info.info.icon;
			npc.GetComponentInChildren<RecruitAnimal> ().Deactivate ();
			Destroy (npc.gameObject);

			CreateAnimalTile (storedNPC, true);
		}


		/// <summary>
		/// Removes all shown tiles in the sort menu
		/// </summary>
		public void ClearTiles()
		{
			foreach (AnimalTile tile in animalListContent.GetComponentsInChildren<AnimalTile>())
			{
				Destroy(tile.gameObject);
			}
		}


		protected AnimalTile AniTile;
		protected GameObject newTile;
		/// <summary>
		/// Creates a new AnimalTile and stores it for use.
		/// 
		/// Under construction.  Needs work.
		/// Problem:  Too specific to scene 'CollectAndFollowAnimalTestScene'; needs generalization.
		/// </summary>
		public virtual void CreateAnimalTile(AnimalStoredInfo animalStoredInfo, bool isNewTile)
		{
			if (animalStoredInfo == null)
				return;

			newTile = Instantiate<GameObject> (animalTilePrefab);	// the prefab must have AnimalTile

			if (!newTile.GetComponent<AnimalTile> ())
			{
				string errMsg = "[AnimalStorageUI.cs]  Whoops!  You tried creating a new Animal Tile in the animal storage process, and the prefab you spawned does not include the 'AnimalTile' component!";
				OutputLog.Write (errMsg);
				Debug.LogError (errMsg);
			}

			AniTile = newTile.GetComponent<AnimalTile> ();	// pull script from spawned object

			// =====

			// Set the animal sprite, gender and index in the script data
			AniTile.animalImage.sprite = animalStoredInfo.icon;
			AniTile.sexIcon.sprite = animalStoredInfo.sex.ToLower() == "female" ? References.instance.sprites["femalesymbol"] : References.instance.sprites["malesymbol"];

			if (isNewTile)
			{
				AniTile.index = AnimalStorage.instance.animalInfoDictionary.Count + 1;
				// Update animalStoredInfo's index to match the tile, then store this index with a reference into memory connecting the tile to the NPC.
				animalStoredInfo.index = AniTile.index;
			}
			else
			{
				AniTile.index = animalStoredInfo.index;
			}


			// If we call this function from an "Add" button, or similar, we modify the storage to now include the new information.
			if (!AnimalStorage.instance.animalInfoDictionary.ContainsKey (animalStoredInfo.index))
				AnimalStorage.instance.animalInfoDictionary.Add (animalStoredInfo.index, animalStoredInfo);

			AniTile.info = animalStoredInfo;


			// Parent the tile to the menu viewport
			newTile.transform.SetParent (animalListContent);
			newTile.transform.localScale = Vector3.one;


			// grab the gameObject references and set to specific components
			newTile.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = AniTile.info.animalName;	// name on tile
			newTile.transform.GetChild (2).GetComponent<Image> ().sprite = AniTile.sexIcon.sprite;						// gender icon on tile

		}


		public virtual void SortBy(string sortTag)
		{
			// Spawn in current tiles
			if (AnimalStorage.instance.animalInfoDictionary != null)
			{
				ClearTiles ();
				Dictionary<int, AnimalStoredInfo> newDict = AnimalStorage.instance.SortAnimalInfoDictBy (AnimalStorage.instance.animalInfoDictionary, sortTag);
				foreach (KeyValuePair<int, AnimalStoredInfo> kvp in newDict)
				{
					CreateAnimalTile (kvp.Value, false);
				}
			}
		}

		/// <summary>
		/// Stores a specific 'AnimalTile' to the current slot.
		/// The 'currentAnimalTile' slot is used to display information visually.
		/// </summary>
		/// <param name="tile">Tile.</param>
		protected void SetCurrentAnimalTile(AnimalTile tile)
		{
			currentAnimalTile = tile;
		}


		/// <summary>
		/// Accessed from a position where specific animals are selected in some way.  
		/// 'Retrieved' animal is spawned into the game world with stored information applied to it.
		/// Animal object is placed in the next available spawn location, usually a known location near the accessed interactable.
		/// A warning is displayed if all spawn locations are full.  Animals cannot be spawned in locations other than spawn locations.
		/// Calls 'EmptyComputerInfo()'
		/// </summary>
		public virtual void RetrieveAnimal()
		{
			if (!currentAnimalTile)
				return;

			// ...
		}

		/// <summary>
		/// Nothing permanent:  Sets all text and values displayed to be either null or zero values.
		/// </summary>
		public virtual void EmptyComputerInfo()
		{
			SetStatValue ("attack", 	 0);
			SetStatValue ("vitality", 	 0);
			SetStatValue ("armor", 		 0);
			SetStatValue ("speed", 		 0);
			SetStatValue ("endurance",   0);
			SetStatValue ("sneak", 		 0);
			SetStatValue ("consumption", 0);
			SetStatValue ("size", 		 0);
		}

		public virtual void ToggleChangeDemoAnimal(bool toggle, AnimalTile aniTile)
		{
			// ...
		}
	}
}