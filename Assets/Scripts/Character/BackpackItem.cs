using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackpackItem : MonoBehaviour {

	[SerializeField] SpriteRenderer _spriteRenderer;

	public void SetContent(Tile tile) {
		
		_spriteRenderer.sprite = tile.sprite;
	}
}