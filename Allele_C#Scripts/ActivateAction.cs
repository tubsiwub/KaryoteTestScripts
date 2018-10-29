using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Used with Tab Target activation

// When tab-targetting an interactable, the selected animal will run over when commanded and invoke this activate event.
// Place this script on any interactable that can be tab-targetted and activated.

namespace Allele
{
	public class ActivateAction : C_Monobehaviour 
	{
		public UnityEvent activate;

		public void InvokeEvent(UnityEvent e)
		{
			e.Invoke ();
		}

		void OnDestroy()
		{
			activate.RemoveAllListeners ();
		}
	}
}
