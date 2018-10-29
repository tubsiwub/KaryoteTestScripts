using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;

namespace Allele
{
	public static class AnimalXMLReader 
	{
		public static void GetAnimalData (out Animal animal, string fileName, string animalName)
		{
			bool debug = false;
			if(debug) Debug.Log ("Get Animal Data... Animal Name: " + animalName + ", File Name: " + fileName);

			// Get gene information from Reference.cs
			int numChromosomes = References.instance.numChromosomes;
			int[] numGenes = References.instance.numGenes;
			int numMitoGenes = References.instance.numMitoGenes;
			int numGenderGenes = References.instance.numGenderGenes;

			TextAsset xmlAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
			MemoryStream memStream = new MemoryStream (xmlAsset.bytes);
			XmlTextReader CReader = new XmlTextReader(memStream);

			animal = new Animal ();
			animal.name = animalName;
			List<Chromosome> femaleList = new List<Chromosome> ();
			List<Chromosome> maleList = new List<Chromosome> ();
			List<Chromosome> mitoList = new List<Chromosome> ();
			Chromosome chromosome = new Chromosome ();
			Gene gene = new Gene ();
			bool foundAnimal = false;

			int gender = 0;

			while (CReader.Read ()) {

				// While data is relevant...
				if (CReader.Name != "" &&
				    CReader.NodeType != XmlNodeType.EndElement &&
				    CReader.Name != "xml" &&
				    foundAnimal) {

					if (CReader.Name == "FemaleList")
					{
						if (debug) Debug.Log ("Found female list");
						for (int i = 0; i < numChromosomes+1; i++)	// chromosome count
						{
							CReader.Read (); CReader.Read ();

							if (i < numChromosomes)
							{
								chromosome = new Chromosome ();
								if (debug)
									Debug.Log ("(F) Create new Chromosome");
								for (int j = 0; j < numGenes[i]; j++)	// gene count
								{
									CReader.Read (); CReader.Read ();
									gene = new Gene ();
									if (debug)
										Debug.Log ("(F) Create new Gene");
									gene.parentOrigin = CReader.GetAttribute ("Parent");
									gene.name = CReader.GetAttribute ("Name");
									gene.allele = float.Parse (CReader.GetAttribute ("Allele"));
									gene.unityID = int.Parse (CReader.GetAttribute ("UnityID"));
									gene.geneNum = int.Parse (CReader.GetAttribute ("GeneID"));
									gene.whichChromosomeGroup = int.Parse (CReader.GetAttribute ("CGroup"));
									gene.whichChromosome = int.Parse (CReader.GetAttribute ("CSpecific"));
									gene.positionWithinChromosome = int.Parse (CReader.GetAttribute ("CPosition"));
									gene.gender = 0;
									chromosome.AddToGeneList (gene);
								}
								femaleList.Add (chromosome);
								if (debug)
									Debug.Log ("Female List: " + femaleList.Count + " : " + femaleList [i].GetGeneList ().Count);
							}
							else
							{
								if (CReader.Name == "X")
									gender = 0;
								else
									gender = 1;

								chromosome = new Chromosome ();
								if (debug)
									Debug.Log ("(F) Create new Chromosome");
								for (int j = 0; j < numGenderGenes; j++)	// gene count
								{
									CReader.Read (); CReader.Read ();
									gene = new Gene ();
									if (debug)
										Debug.Log ("(F) Create new Gene");
									gene.parentOrigin = CReader.GetAttribute ("Parent");
									gene.name = CReader.GetAttribute ("Name");
									gene.allele = float.Parse (CReader.GetAttribute ("Allele"));
									gene.unityID = int.Parse (CReader.GetAttribute ("UnityID"));
									gene.geneNum = int.Parse (CReader.GetAttribute ("GeneID"));
									gene.whichChromosomeGroup = int.Parse (CReader.GetAttribute ("CGroup"));
									gene.whichChromosome = int.Parse (CReader.GetAttribute ("CSpecific"));
									gene.positionWithinChromosome = int.Parse (CReader.GetAttribute ("CPosition"));
									gene.gender = gender;
									chromosome.AddToGeneList (gene);
								}
								femaleList.Add (chromosome);
								if (debug)
									Debug.Log ("X Chromosome List: " + femaleList.Count + " : " + femaleList [numChromosomes].GetGeneList ().Count);
							}

							CReader.Read ();CReader.Read ();
						}
					}
					if (CReader.Name == "MaleList")
					{
						if (debug) Debug.Log ("Found male list");
						for (int i = 0; i < numChromosomes+1; i++)
						{
							CReader.Read (); CReader.Read ();

							if (i < numChromosomes)
							{
								chromosome = new Chromosome ();
								if (debug)
									Debug.Log ("(M) Create new Chromosome");
								for (int j = 0; j < numGenes[i]; j++)	// gene count
								{
									CReader.Read ();
									CReader.Read ();
									gene = new Gene ();
									if (debug)
										Debug.Log ("(M) Create new Gene");
									gene.parentOrigin = CReader.GetAttribute ("Parent");
									gene.name = CReader.GetAttribute ("Name");
									gene.allele = float.Parse (CReader.GetAttribute ("Allele"));
									gene.unityID = int.Parse (CReader.GetAttribute ("UnityID"));
									gene.geneNum = int.Parse (CReader.GetAttribute ("GeneID"));
									gene.whichChromosomeGroup = int.Parse (CReader.GetAttribute ("CGroup"));
									gene.whichChromosome = int.Parse (CReader.GetAttribute ("CSpecific"));
									gene.positionWithinChromosome = int.Parse (CReader.GetAttribute ("CPosition"));
									gene.gender = 0;
									chromosome.AddToGeneList (gene);
								}
								maleList.Add (chromosome);
								if (debug)
									Debug.Log ("Male List: " + maleList.Count + " : " + maleList [0].GetGeneList ().Count);
							}
							else
							{
								if (CReader.Name == "X")
									gender = 0;
								else
									gender = 1;

								chromosome = new Chromosome ();
								if (debug)
									Debug.Log ("(F) Create new Chromosome");
								for (int j = 0; j < numGenderGenes; j++)	// gene count
								{
									CReader.Read (); CReader.Read ();
									gene = new Gene ();
									if (debug)
										Debug.Log ("(F) Create new Gene");
									gene.parentOrigin = CReader.GetAttribute ("Parent");
									gene.name = CReader.GetAttribute ("Name");
									gene.allele = float.Parse (CReader.GetAttribute ("Allele"));
									gene.unityID = int.Parse (CReader.GetAttribute ("UnityID"));
									gene.geneNum = int.Parse (CReader.GetAttribute ("GeneID"));
									gene.whichChromosomeGroup = int.Parse (CReader.GetAttribute ("CGroup"));
									gene.whichChromosome = int.Parse (CReader.GetAttribute ("CSpecific"));
									gene.positionWithinChromosome = int.Parse (CReader.GetAttribute ("CPosition"));
									gene.gender = gender;
									chromosome.AddToGeneList (gene);
								}
								maleList.Add (chromosome);
								if (debug)
									Debug.Log ("Y Chromosome List: " + maleList.Count + " : " + maleList [numChromosomes].GetGeneList ().Count);
							}

							CReader.Read ();CReader.Read ();
						}
					}
					if (CReader.Name == "Mitochondrial")
					{
						if (debug) Debug.Log ("Found mitochondrial");
						CReader.Read (); CReader.Read ();
						chromosome = new Chromosome ();
						if(debug) Debug.Log ("(M) Create new Mito Chromosome");
						for (int j = 0; j < numMitoGenes; j++)	// gene count
						{
							CReader.Read (); CReader.Read ();
							gene = new Gene ();
							if (debug) Debug.Log ("(M) Create new Mito Gene");
							gene.parentOrigin = CReader.GetAttribute ("Parent");
							gene.name = CReader.GetAttribute ("Name");
							gene.allele = float.Parse(CReader.GetAttribute ("Allele"));
							gene.unityID = int.Parse(CReader.GetAttribute ("UnityID"));
							gene.geneNum = int.Parse(CReader.GetAttribute ("GeneID"));
							gene.whichChromosomeGroup = int.Parse(CReader.GetAttribute ("CGroup"));
							gene.whichChromosome = int.Parse(CReader.GetAttribute ("CSpecific"));
							gene.positionWithinChromosome = int.Parse(CReader.GetAttribute ("CPosition"));
							gene.gender = 0;
							chromosome.AddToGeneList (gene);
						}
						mitoList.Add (chromosome);
						if (debug) Debug.Log ("Mito List: " + mitoList.Count + " : " + mitoList[0].GetGeneList().Count);
						CReader.Read ();CReader.Read ();
					}
				}

				if (CReader.Name == animalName)
				{
					foundAnimal = true;
					if (debug) Debug.Log ("Found Animal");
				}
			}

			animal.SetChromosomes (femaleList, maleList);
			animal.mitochondrial = mitoList [0];
			animal.gender = gender;
			animal.InitializeStoredTraitList ();
			animal.InitializeBlendshapesDict ();

			// Set the animal's blends without using a gameObject - not sure why I did this.  Commented out:  8/1/2018, worked on animal colors with new animal rig
			//foreach (Trait trait in References.instance.traitList.GetTraitList())
			//{
			//	trait.ModifyAnimal (animal, femaleList, maleList);
			//}

			if (debug) Debug.Log ("Female Totals: Chromosomes - " + femaleList.Count + ",  Genes (first chromosome) - " + femaleList[0].GetGeneList().Count);
			if (debug) Debug.Log ("Special Chromosome (Female) Genes - " + femaleList[numChromosomes].GetGeneList().Count);
			if (debug) Debug.Log ("Male Totals: Chromosomes - " + maleList.Count + ",  Genes (first chromosome) - " + maleList[0].GetGeneList().Count);
			if (debug) Debug.Log ("Special Chromosome (Male) Genes - " + maleList[numChromosomes].GetGeneList().Count);
			if (debug) Debug.Log ("Mitochondrial Chromosome - " + animal.mitochondrial + ",  Genes (first chromosome) - " + animal.mitochondrial.GetGeneList().Count);
			if (debug) Debug.Log ("...");

			CReader.Close ();
		}
	}
}