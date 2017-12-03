using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSource : MonoBehaviour {

	[SerializeField] TilemapProvider _tilemapProvider;
	[Space]
	[SerializeField] Tilemap _tilemap;

	public void Awake() {

		_tilemapProvider.Init(_tilemap);
	}
}
