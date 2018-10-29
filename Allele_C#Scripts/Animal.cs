using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Allele
{
	[Serializable]
	public class Animal
	{
		public string name;
		public int gender;						// 0 = female, 1 = male

		public enum BodyType
		{
			DOG,
			EQUINE,
			BIPED
		}
		public BodyType bodyType;

		[HideInInspector]
		public Chromosome mitochondrial;

		List<Chromosome> chromosomesFemale;		// Left
		List<Chromosome> chromosomesMale;		// Right

		[Serializable]	// will be replaced with blendShapes Dict. below
		public struct StoredTrait
		{
			public StoredTrait(string n)
			{
				trait = new Trait();
				name = n;
				value_v2 = new List<Vector2>();
				value_f = new List<float>();
			}

			public Trait trait;
			public string name;
			public List<Vector2> value_v2;
			public List<float> value_f;
		}

		public Dictionary<int, float> blendShapes;	// index, value

		// Stored trait info
		public List<StoredTrait> storedTrait;

		public Animal()
		{
			
		}

		public void InitializeBlendshapesDict()
		{
			blendShapes = new Dictionary<int, float> ();

			int blendShapeTotal = References.instance.totalBlendShapes;
			for(int i = 0; i < blendShapeTotal; i++)
				blendShapes.Add (i, 0);
		}

		public void SetBlendshapeDictValue(int index, float value)
		{
			blendShapes [index] = value;
		}

		// will be replaced with blendShapes Dict. above
		public void InitializeStoredTraitList()
		{
			storedTrait = new List<StoredTrait> ();

			// Just a list; doesn't need to match the XML chromosome ordering
			storedTrait.Add(new StoredTrait ("bodytype"));				// 0		
			storedTrait.Add(new StoredTrait ("browtype"));				// 1		
			storedTrait.Add(new StoredTrait ("browheight"));			// 2		
			storedTrait.Add(new StoredTrait ("buffalohump"));			// 3		
			storedTrait.Add(new StoredTrait ("cheektype"));				// 4		
			storedTrait.Add(new StoredTrait ("eumelanin"));				// 5		
			storedTrait.Add(new StoredTrait ("pheomelanin"));			// 6		
			storedTrait.Add(new StoredTrait ("bluerefraction"));		// 7		
			storedTrait.Add(new StoredTrait ("dragonfin"));				// 8		
			storedTrait.Add(new StoredTrait ("eartype"));				// 9		
			storedTrait.Add(new StoredTrait ("foottype"));				// 10		
			storedTrait.Add(new StoredTrait ("snouttype"));				// 11		
			storedTrait.Add(new StoredTrait ("tailtype"));				// 12		
			storedTrait.Add(new StoredTrait ("vocalsac"));				// 13		
			storedTrait.Add(new StoredTrait ("ocularmelanin"));			// 14		
			storedTrait.Add(new StoredTrait ("eyecolor"));				// 15		
			storedTrait.Add(new StoredTrait ("eyeveindensity"));		// 16		
			storedTrait.Add(new StoredTrait ("blackspotting"));			// 17		
			storedTrait.Add(new StoredTrait ("blackspottingamount"));	// 18		
			storedTrait.Add(new StoredTrait ("whitespotting"));			// 19		
			storedTrait.Add(new StoredTrait ("albinism"));				// 20		
			storedTrait.Add(new StoredTrait ("texture"));				// 21		

			foreach (Trait trait in References.instance.traitList.GetTraitList())
			{
				trait.Init ();
				trait.StoreTrait (this, chromosomesFemale, chromosomesMale);
			}
		}

		public virtual List<Chromosome> GetFemaleList() { return chromosomesFemale; }
		 
		public virtual List<Chromosome> GetMaleList() { return chromosomesMale; }
		 
		public virtual void SetChromosomes(List<Chromosome> a, List<Chromosome> b)
		{
			chromosomesFemale = a;
			chromosomesMale = b;
		}

		public virtual void SetChromosomes(List<Chromosome> a, List<Chromosome> b, Chromosome c)
		{
			chromosomesFemale = a;
			chromosomesMale = b;
			mitochondrial = c;
		}
	}
}