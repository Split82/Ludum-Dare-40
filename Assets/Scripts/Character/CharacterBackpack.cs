using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterBackpack : MonoBehaviour {

	[SerializeField] CharacterLevelInteraction _characterLevelInteraction;	
	[SerializeField] BackpackItem _backPackItemPrefab;

	private List<BackpackItem> _backpackItems;

	private void Awake() {

		ObjectPool.CreatePool(_backPackItemPrefab, 10, transform);

		_backpackItems = new List<BackpackItem>(10);
	}

	private void OnEnable() {
		
		_characterLevelInteraction.didPickupTileEvent += HandleCharacterDidPickupTileEvent;
	}

	private void OnDisable() {

		_characterLevelInteraction.didPickupTileEvent -= HandleCharacterDidPickupTileEvent;
	}

	private void HandleCharacterDidPickupTileEvent(Tile tile) {

		var newBackpackItem = _backPackItemPrefab.Spawn();		
		newBackpackItem.SetContent(tile);
		_backpackItems.Add(newBackpackItem);
	}
}
