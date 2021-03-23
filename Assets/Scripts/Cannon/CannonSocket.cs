using UnityEngine;

public class CannonSocket : MonoBehaviour
{
	[SerializeField] private CannonPartType type;

	public CannonPartType Type => type;
}