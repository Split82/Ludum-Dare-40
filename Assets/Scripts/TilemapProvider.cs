using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProvider : ScriptableObject {

	private Tilemap _tilemap;

	public Tilemap tilemap {
		get {
			return _tilemap;
		}
	}

	public void Init(Tilemap tilemap) {
		
		_tilemap = tilemap;
	}
}
