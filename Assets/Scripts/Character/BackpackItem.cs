using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackpackItem : MonoBehaviour {

	[SerializeField] SpriteRenderer _spriteRenderer;

	public Tile tile {
		get {
			return _tile;
		}		
	}

	private Tile _tile;	

	public void SetContent(Tile tile) {

		_tile = tile;
		_spriteRenderer.sprite = tile.sprite;
	}

	public void SetLocalPosition(Vector3 position) {
		
		transform.localPosition = position;
	}
}