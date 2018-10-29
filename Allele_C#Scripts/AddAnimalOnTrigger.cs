using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public class AddAnimalOnTrigger : C_Monobehaviour 
	{
		Breeding breeding;

		public Animal animal;

		public int chromoCount;

		public List<Gene> genes;

		List<Chromosome> chromo1, chromo2;
		
		void Start () 
		{
			breeding = GameObject.FindWithTag ("Breeding").GetComponent<Breeding> ();

			chromo1 = new List<Chromosome> ();
			chromo2 = new List<Chromosome> ();

			for (int i = 0; i < chromoCount; i++)
			{
				Chromosome newChromosome = new Chromosome ();
				for (int j = 0; j < genes.Count; j++)
				{
					newChromosome.AddToGeneList (genes [j]);
				}
				chromo1.Add (newChromosome);
			}


			for (int i = 0; i < chromoCount; i++)
			{
				Chromosome newChromosome = new Chromosome ();
				for (int j = 0; j < genes.Count; j++)
				{
					newChromosome.AddToGeneList (genes [j]);
				}
				chromo2.Add (newChromosome);
			}

			animal.SetChromosomes (chromo1, chromo2);
		}
		
		void OnTriggerEnter (Collider col) 
		{
			breeding.StoreOutputAnimal (animal);
			Destroy (this.gameObject);
		}
	}
}