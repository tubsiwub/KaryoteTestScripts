using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Allele
{
	public static class AnimalExtensions
	{
		// Different way to resetting an animal of type<T> to type<Animal> and clearing data within
		public static Animal Reset(this Animal val)
		{
			return val = new Animal();
		}
	}
}