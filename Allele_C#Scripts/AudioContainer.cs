using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public class AudioContainer : C_Monobehaviour 
	{
															
		public AudioSource[] AuSo;							//							PlayerSounds		 ItemSounds			  AnimalSounds		   Extra 1				Extra 2
		public Dictionary<string, AudioSource> audioSource;	// areaMusic, areaAmbience, soundEffectChannel0, soundEffectChannel1, soundEffectChannel2, soundEffectChannel3, soundEffectChannel4, vocalTrack0, vocalTrack1

		public static AudioContainer instance;
		void Start () 
		{
			#region Singleton
			// Singleton
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);
			#endregion


			audioSource = new Dictionary<string, AudioSource> ();
			audioSource.Add ("areamusic"			, AuSo[0]);
			audioSource.Add ("areaambience"			, AuSo[1]);
			audioSource.Add ("soundeffectchannel0"	, AuSo[2]);
			audioSource.Add ("soundeffectchannel1"	, AuSo[3]);
			audioSource.Add ("soundeffectchannel2"	, AuSo[4]);
			audioSource.Add ("soundeffectchannel3"	, AuSo[5]);
			audioSource.Add ("soundeffectchannel4"	, AuSo[6]);
			audioSource.Add ("vocaltrack0"			, AuSo[7]);
			audioSource.Add ("vocaltrack1"			, AuSo[8]);

			//StartCoroutine (RestoreLevels (2.0f));	// *1
		}

		// Continuous *1
		IEnumerator RestoreLevels(float resetInterval)
		{
			while (true)
			{
				foreach (KeyValuePair<string, AudioSource> entry in audioSource)
				{
					if (!entry.Value.isPlaying)
					{
						entry.Value.volume = 1;
						entry.Value.pitch = 1;
						entry.Value.panStereo = 0;
					}
				}

				for (int i = 0; i < AuSo.Length; i++)
				{
					if (!AuSo [i].isPlaying)
					{
						AuSo [i].volume = 1;
						AuSo [i].pitch = 1;
						AuSo [i].panStereo = 0;
					}
				}

				yield return new WaitForSeconds (resetInterval);
				//yield return new WaitForEndOfFrame ();
			}
		}

		// Initiated
		IEnumerator RestoreLevels(string soundName, string audioChannel)
		{
			while (audioSource [audioChannel].isPlaying)
			{
				yield return new WaitForEndOfFrame ();
			}

			audioSource [audioChannel].volume = 1;
			audioSource [audioChannel].pitch = 1;
			audioSource [audioChannel].panStereo = 0;
		}

		IEnumerator RestoreLevels(string soundName, int audioChannel)
		{
			while (AuSo [audioChannel].isPlaying)
			{
				yield return new WaitForEndOfFrame ();
			}

			AuSo [audioChannel].volume = 1;
			AuSo [audioChannel].pitch = 1;
			AuSo [audioChannel].panStereo = 0;
		}

		// Ordinary play using default values
		public void PlaySound(string soundName, float volume, string audioChannel)
		{
			audioSource [audioChannel].PlayOneShot (References.instance.audioClips [soundName.ToLower()], volume);
		}

		public void PlaySound(string soundName, float volume, int audioChannel)
		{
			AuSo [audioChannel].PlayOneShot (References.instance.audioClips [soundName.ToLower()], volume);
		}

		// Play using specifics
		public void PlaySound(string soundName, float volume, string audioChannel, float pitch, float pan)
		{
			audioSource [audioChannel].pitch = pitch;
			audioSource [audioChannel].panStereo = pan;
			audioSource [audioChannel].PlayOneShot (References.instance.audioClips [soundName.ToLower()], volume);
			StartCoroutine(RestoreLevels (soundName, audioChannel));
		}

		public void PlaySound(string soundName, float volume, int audioChannel, float pitch, float pan)
		{
			AuSo [audioChannel].pitch = pitch;
			AuSo [audioChannel].panStereo = pan;
			AuSo [audioChannel].PlayOneShot (References.instance.audioClips [soundName.ToLower()], volume);
			StartCoroutine(RestoreLevels (soundName, audioChannel));
		}


		public void StopSoundChannel(string audioChannel)
		{
			audioSource [audioChannel].Stop ();
		}

		public void StopSoundChannel(int audioChannel)
		{
			AuSo [audioChannel].Stop ();
		}
	}
}