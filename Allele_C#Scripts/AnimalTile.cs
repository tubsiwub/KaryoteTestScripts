using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Allele
{
	[RequireComponent(typeof(Button))]
	public class AnimalTile : C_Monobehaviour 
	{
		[Header("ANIMAL")]
		public int index;
		public AnimalStoredInfo info;

		public Image animalImage; 
		public Image sexIcon; 

		void OnDestroy()
		{
			GetComponent<Button> ().onClick.RemoveAllListeners ();
		}
	}
}