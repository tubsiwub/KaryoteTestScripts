using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Used in breeding and creature storage.
// Is used ALL OVER the place for breeding - definitely don't delete anything in here.
// Fun Fact: Was the first class to have data saved to SaveLoad, which is neat.
using System.Collections.Specialized;

namespace Allele
{
	public class AnimalStorage : C_Monobehaviour 
	{
		#region Creature Storage

		public Dictionary<int, AnimalStoredInfo> animalInfoDictionary;	// !! reminder:  make this serializable.

		#endregion

		#region Breeding-specific
		public List<Animal> storedAnimals;

		public Animal selectedMother;
		public Animal selectedFather;
		#endregion

		// Singleton
		public static AnimalStorage instance = null;

		void Awake () 
		{
			#region Singleton
			// Singleton
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);
			#endregion

			#region Creature Storage

			// If null, we either didn't load in the data or we have a wipe data check.  Either way, create a new one.
			if(animalInfoDictionary == null)
				animalInfoDictionary = new Dictionary<int, AnimalStoredInfo>();

			#endregion

			#region Breeding-specific
			storedAnimals = new List<Animal> ();

			selectedMother = null;
			selectedFather = null;
			#endregion

			string[] words = { "cherr", "hlaal3", "pooty44" };

			var query = from w in words
			            select w.Length;

			Debug.Log (query.Min ());
		}

		#region Creature Storage



		#endregion

		#region Breeding-specific
		public void AddAnimal(
			string name,
			int gender,
			List<Chromosome> femaleChromosomeList,
			List<Chromosome> maleChromosomeList)
		{
			Animal newAnimal = new Animal ();

			newAnimal.name = name;
			newAnimal.gender = gender;
			newAnimal.SetChromosomes(femaleChromosomeList, maleChromosomeList);

			storedAnimals.Add (newAnimal);
		}

		public void AddAnimal(Animal a)
		{
			Animal newAnimal = a;

			if (HasList)
				storedAnimals.Add (newAnimal);
		}

		public int ListLength { get { return storedAnimals.Count; } }

		public bool HasList { get { return storedAnimals != null; } }

		public Animal GetByIndex(int index)
		{
			return storedAnimals[index];
		}
		#endregion

	
		public Dictionary<int, AnimalStoredInfo> SortAnimalInfoDictBy(Dictionary<int, AnimalStoredInfo> sortDict, string sortTag)
		{
			Dictionary<int, AnimalStoredInfo> newDict = animalInfoDictionary;

			var list = newDict.ToList();

			list.Sort (new SortAnimalDictByValueCompare_Specific (sortTag));

			foreach (var key in list)
				Debug.Log ("Key: " + key.Key + ", Value: " + key.Value + ", Dict. Value: " + newDict[key.Key]);

			newDict = list.ToDictionary (x => x.Key, x => x.Value);

			return newDict;
		}

		public class SortAnimalDictByValueCompare_Specific : IComparer<KeyValuePair<int, AnimalStoredInfo>>
		{
			string sortByStat;

			public SortAnimalDictByValueCompare_Specific(string s)
			{
				sortByStat = s;
			}

			public int Compare(KeyValuePair<int, AnimalStoredInfo> x, KeyValuePair<int, AnimalStoredInfo> y)
			{			
				// Does the AnimalStoredInfo exist in the dictionary?
				int nullCheck = NullCheck <AnimalStoredInfo> (x.Value, y.Value);
				if(nullCheck != 2) 
					return nullCheck;

				// If so, compare the specific stat
				int difference = 0;
				switch(sortByStat.ToLower())
				{
					case "size":
						difference = y.Value.size.CompareTo (x.Value.size);
						break;

					case "recent":
						difference = x.Key.CompareTo (y.Key);
						break;

					case "alphabet":
						nullCheck = NullCheck<string> (x.Value.animalName, y.Value.animalName);
						if(nullCheck != 2) 
							return nullCheck;
						else
							difference = x.Value.animalName.CompareTo(y.Value.animalName);
						break;

					case "attack":
						difference = y.Value.attack.CompareTo (x.Value.attack);
						break;

					case "vitality":
						difference = y.Value.vitality.CompareTo (x.Value.vitality);
						break;

					case "defense":
						difference = y.Value.armor.CompareTo (x.Value.armor);
						break;

					case "speed":
						difference = y.Value.speed.CompareTo (x.Value.speed);
						break;

					case "sneak":
						difference = y.Value.sneak.CompareTo (x.Value.sneak);
						break;

					case "endurance":
						difference = y.Value.endurance.CompareTo (x.Value.endurance);
						break;
				}

				if (difference != 0) {
					return difference;
				}  else {
					return 0;
				}
			}

			int NullCheck<T>(T x, T y)
			{
				if (x == null) {
					if (y == null) {
						return 0;
					}  else {
						return 1;
					}
				}  else {
					if (y == null) {
						return -1;
					}  else {
						return 2;
					}
				}
			}
		}

	}
}