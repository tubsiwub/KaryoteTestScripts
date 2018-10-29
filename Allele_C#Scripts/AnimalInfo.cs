using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Allele
{
	[Serializable]
	public class AnimalStoredInfo
	{
		public int index;

		public CreatureStorage_DemoAnimalControl.Model modelType;
		public float[] blendshapes;

		[Header("Images")]
		public Sprite icon;

		[Header("Name / Sex")]
		public string animalName;
		public string sex;

		[Header("Attributes")]
		public int attack;
		public int vitality;
		public int armor;
		public int speed;
		public int endurance;
		public int sneak;
		public int size;		
		public int consumption;	

		public void SetInfo(
			string _animalName,
			string _sex,
			int _attack,
			int _vitality,
			int _armor,
			int _speed,
			int _endurance,
			int _sneak,
			int _size,
			int _consumption)
		{
			animalName = _animalName;
			sex = _sex;
			attack = _attack;
			vitality = _vitality;
			armor = _armor;
			speed = _speed;
			endurance = _endurance;
			sneak = _sneak;
			size = _size;
			consumption = _consumption;
		}
	}

	public class AnimalInfo : MonoBehaviour 
	{
		public int stallNum = -1;
		public AnimalStoredInfo info;

		void Start()
		{
			if (GetComponentInChildren<SkinnedMeshRenderer> ())
			{
				SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer> ();
				float[] newArray = new float[References.instance.totalBlendShapes];
				for (int i = 0; i < References.instance.totalBlendShapes; i++)
				{
					newArray [i] = smr.GetBlendShapeWeight (i);
				}
				info.blendshapes = newArray;
			}
			else
			{
				OutputLog.Write ("Spawned animal " + this.name + " does not have a SkinnedMeshRenderer attached to any of its parts.");
				Debug.LogError ("Spawned animal " + this.name + " does not have a SkinnedMeshRenderer attached to any of its parts.", this);
			}
		}
	}
}
