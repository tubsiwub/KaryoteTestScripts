using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace Allele
{
	[CustomEditor(typeof(AnimalStorage))]
	public class AnimalStorage_editor : Editor 
	{
		public override void OnInspectorGUI()
		{
			// Update the object this script is modifying
			serializedObject.Update ();

			AnimalStorage mainScript = target as AnimalStorage;

			// Show script reference
			GUI.enabled = false;
			EditorGUILayout.ObjectField ("Script:", MonoScript.FromMonoBehaviour (mainScript), typeof(MonoScript), false);
			GUI.enabled = true;

			// ============= MAIN CODE =====================


			// ...


			// =============================================

			// Updates property values as changes are made within this script
			serializedObject.ApplyModifiedProperties ();
		}
	}
}
