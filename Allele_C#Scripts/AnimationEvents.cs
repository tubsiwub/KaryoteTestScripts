using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Allele
{
	public class AnimationEvents : C_Monobehaviour 
	{
		public UnityEvent _enable, _disable;
		public UnityEvent _animationStart, _animationEnd, _allAnimationsEnd;

		public Animator anim;

		bool animatorEnabled = false;
		int clipCount = 0;

		void Start()
		{
			animatorEnabled = anim.enabled;
			clipCount = anim.GetCurrentAnimatorClipInfo (0).Length;
		}

		void LateUpdate()
		{
			// check status of animator
			if (animatorEnabled != anim.enabled)
			{
				if (anim.enabled)
					_enable.Invoke ();
				else
					_disable.Invoke ();
				
				animatorEnabled = anim.enabled;
			}

			int currentClipCount = anim.GetCurrentAnimatorClipInfo (0).Length;
			if (clipCount != currentClipCount)
			{
				if (currentClipCount > clipCount)
					_animationStart.Invoke ();
				else if (currentClipCount < clipCount)
					_animationEnd.Invoke ();

				if (currentClipCount <= 0)
					_allAnimationsEnd.Invoke ();
				
				clipCount = currentClipCount;
			}
		}
	}
}
