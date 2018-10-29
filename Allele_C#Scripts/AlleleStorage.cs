using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace Allele
{
	[Serializable]
	public class AlleleStorage : C_Monobehaviour 
	{
		public Transform geneContainer;
		public GameObject genePrefab;
		public TextMeshProUGUI currentGeneText;

		[Serializable]
		public enum InheritanceType { Null, Dominant, Recessive, Incomplete, Codominant, Equal }

		[Serializable]
		public struct Allele
		{
			public string name;

			public int index;

			public List<SequencedAllele> sequencedAlleles;
		}

		[Serializable]
		public struct SequencedAllele
		{
			public string name;

			public InheritanceType[] correctCompareList;
			public InheritanceType[] userCompareList;

			[RangeAttribute(0.0f,1.0f)]
			public float value;	// specific allele value from code.

			public int rank;	// compares to other SequencedAlleles.  Higher is more dominant.

			public int number;	// number in the master-list of alleles.

			public bool known;	// true if the player has discovered this gene.  Allows it to be shown and used from the list.
		}

		public List<Allele> alleleList;	// all alleles in the game containing their specific varieties.
		public Allele currentAllele;
		Allele lastAllele;

		public static AlleleStorage instance;

		void Start () 
		{
			#region Singleton
			if(instance == null)
				instance = this;
			else if(instance != this)
				Destroy(instance);
			#endregion

			PopulateList ();
		}
		
		void Update () 
		{
			CheckChanges ();
		}

		void CheckChanges()
		{
			if (lastAllele.name != currentAllele.name)
			{
				PopulateList ();
			}
			lastAllele = currentAllele;
		}

		void PopulateList()
		{
			currentGeneText.text = currentAllele.name;

			foreach (Transform child in geneContainer)
			{
				Destroy (child.gameObject);
			}

			foreach (Allele allele in alleleList)
			{
				if (allele.name == currentAllele.name)
				{
					foreach (SequencedAllele seq in allele.sequencedAlleles)
					{
						GameObject newGene = Instantiate<GameObject> (genePrefab);
						newGene.name = seq.name;
						newGene.GetComponentInChildren<TextMeshProUGUI> ().text = seq.name;

						newGene.GetComponent<DraggableGene> ().typeCompareList = seq.userCompareList;
						newGene.GetComponent<DraggableGene> ().value = seq.value;
						newGene.GetComponent<DraggableGene> ().rank = seq.rank;
						newGene.GetComponent<DraggableGene> ().number = seq.number;
						newGene.GetComponent<DraggableGene> ().known = seq.known;

						newGene.GetComponent<DraggableGene> ().otherRectTransform = new RectTransform[2];
						newGene.GetComponent<DraggableGene> ().otherRectTransform [0] = GameObject.Find ("Choice1").GetComponent<RectTransform> ();
						newGene.GetComponent<DraggableGene> ().otherRectTransform [1] = GameObject.Find ("Choice2").GetComponent<RectTransform> ();
						newGene.GetComponent<DraggableGene> ().canvas = GameObject.Find ("Canvas").transform;
						newGene.GetComponent<DraggableGene> ().compareGene = GameObject.Find ("GeneViewer").GetComponent<CompareGeneChoice> ();

						newGene.transform.SetParent (geneContainer);
					}
					break;
				}
			}
		}
	}
}