using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimalCreator : EditorWindow 
{
	// User Input Animal Values
	string animalName = "";
	int animalGender = 0;

	// User Input Chromosome Values
	//int numberOfGenesPerChromosome = 20;
	int specificGeneDisplay = 4;

	// User Input Gene Values
	//float[] geneAlleleValue;
	//int[] geneNumberValue;
	//int[] geneUnityValue;
	//string[] geneName;
	//string[] geneParentOriginNameValue;
	//int[] geneChromosomeGroup;
	//int[] geneChromosomeSpecific;
	//int[] geneChromosomePosition;


	[MenuItem("Window/Animal Creator")]
	public static void ShowWindow () 
	{
		EditorWindow.GetWindow (typeof(AnimalCreator));
	}

	public void OnInspectorUpdate()
	{
		Repaint ();

		// make custom class and store this data for now
		//geneAlleleValue = new float[numberOfGenesPerChromosome];
		//geneNumberValue = new int[numberOfGenesPerChromosome];
		//geneUnityValue = new int[numberOfGenesPerChromosome];
		//geneName = new string[numberOfGenesPerChromosome];
		//geneParentOriginNameValue = new string[numberOfGenesPerChromosome];
		//geneChromosomeGroup = new int[numberOfGenesPerChromosome];
		//geneChromosomeSpecific = new int[numberOfGenesPerChromosome];
		//geneChromosomePosition = new int[numberOfGenesPerChromosome];
	}

	public void OnGUI()
	{
		GUILayout.Space (12);
		GUILayout.Label ("Animal");
		GUILayout.Space (12);

		EditorGUILayout.BeginHorizontal ();
		animalName = EditorGUILayout.TextField (new GUIContent ("Animal Name", "Name of the creature."), animalName);
		animalGender = EditorGUILayout.IntField (new GUIContent ("Animal Gender", "Animal gender; 0 = female, 1 = male"), animalGender);
		EditorGUILayout.EndHorizontal ();

		specificGeneDisplay = EditorGUILayout.IntField ("GENE #", specificGeneDisplay);

		GUILayout.Space (6);
		GUILayout.Label ("Gene #" + specificGeneDisplay);
		GUILayout.Space (4);

		EditorGUILayout.BeginVertical ();
		//geneAlleleValue[specificGeneDisplay] = EditorGUILayout.FloatField (new GUIContent ("Allele", "Value representing this gene that differentiates it from other genes."), geneAlleleValue[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneNumberValue[specificGeneDisplay] = EditorGUILayout.IntField (new GUIContent ("ID", "Number identifying this gene."), geneNumberValue[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneUnityValue[specificGeneDisplay] = EditorGUILayout.IntField (new GUIContent ("UnityID", "Highly specific to the engine."), geneUnityValue[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneName[specificGeneDisplay] = EditorGUILayout.TextField (new GUIContent ("Name", "Which creature does this gene belong to?"), geneName[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneParentOriginNameValue[specificGeneDisplay] = EditorGUILayout.TextField (new GUIContent ("Parent", "Parent creature that passed this gene on."), geneParentOriginNameValue[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneChromosomeGroup[specificGeneDisplay] = EditorGUILayout.IntField (new GUIContent ("C. Group", "Female/Male Chromosome list"), geneChromosomeGroup[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneChromosomeSpecific[specificGeneDisplay] = EditorGUILayout.IntField (new GUIContent ("C. Specific", "Which Chromosome is this gene a part of?"), geneChromosomeSpecific[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		//geneChromosomePosition[specificGeneDisplay] = EditorGUILayout.IntField (new GUIContent ("C. Position", "Position within Chromosome that this gene is."), geneChromosomePosition[specificGeneDisplay], GUILayout.MaxWidth (200.0f));
		EditorGUILayout.EndVertical ();

	}
}
