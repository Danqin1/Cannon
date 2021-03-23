using System.Collections.Generic;
using UnityEngine;

public class CannonPart : MonoBehaviour
{
	[SerializeField] private CannonPartType type;
	[SerializeField] private List<CannonSocket> sockets;

	public CannonPartType Type => type;
	public List<CannonSocket> Sockets => sockets;
}