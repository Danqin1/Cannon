using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityTemplateProjects
{
	[CreateAssetMenu(fileName = "CannonPartsDatabase", menuName = "Koffeecup/CannonPartsDatabase", order = 0)]
	public class CannonPartsDatabase : ScriptableObject
	{
		public struct CannonPartDataResponse
		{
			public CannonPart part;
			public int index;
		}
		
		[SerializeField] private List<CannonPart> barrels;
		[SerializeField] private List<CannonPart> wheels;
		[SerializeField] private List<CannonPart> stands;

		public List<CannonPart> Barrels => barrels;
		public List<CannonPart> Wheels => wheels;
		public List<CannonPart> Stands => stands;

		public CannonPartDataResponse GetRandomPart(CannonPartType type)
		{
			var response = new CannonPartDataResponse();
			switch (type)
			{
				case CannonPartType.Wheel:
					response.index = Random.Range(0, wheels.Count);
					response.part = wheels[response.index];
					break;
				case CannonPartType.Barrel:
					response.index = Random.Range(0, barrels.Count);
					response.part = barrels[response.index];
					break;
				case CannonPartType.Stand:
					response.index = Random.Range(0, stands.Count);
					response.part = stands[response.index];
					break;
			}

			return response;
		}
	}
}