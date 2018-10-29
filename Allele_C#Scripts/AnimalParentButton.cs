using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public class AnimalParentButton : C_Monobehaviour 
	{
		public int index;

		public void Modify()
		{
			// Store the selection
			int gender = AnimalStorage.instance.GetByIndex (index).gender;

			GameObject animalObj;

			if (gender == 0)
			{
				AnimalStorage.instance.selectedMother = AnimalStorage.instance.GetByIndex (index);
				animalObj = Breeding.instance.motherObj;
			}
			else
			{
				AnimalStorage.instance.selectedFather = AnimalStorage.instance.GetByIndex (index);
				animalObj = Breeding.instance.fatherObj;
			}

			Breeding.instance.ModifyAnimal (AnimalStorage.instance.GetByIndex (index), animalObj);
		}
	}
}