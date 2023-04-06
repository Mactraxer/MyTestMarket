using UnityEngine;

namespace Fruits
{
	[CreateAssetMenu(menuName = "MyTestMarket/FruitData", fileName = "FruitData")]
	public class FruitData : ScriptableObject
	{
		[SerializeField] private string _name;
		[SerializeField] private Material _material;
		[SerializeField] private Mesh _mesh;
		[SerializeField] private float _growTime;
		[SerializeField] private int _price;

		public string Name => _name;
		public Material Material => _material;
		public int Price => _price;
		public Mesh Mesh => _mesh;
		public float GrowTime => _growTime;
	}
}