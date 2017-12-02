using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterBackpack : MonoBehaviour {

	[SerializeField] CharacterLevelInteraction _characterLevelInteraction;	
	[SerializeField] BackpackItem _backPackItemPrefab;

	public int numberOfItemsInBackpack {
		get {
			return _backpackItems.Count;
		}
	}

	public Vector3 topSlotPosition {
		get {
			return transform.TransformPoint(localTopSlotPosition);
		}
	}

	private Vector3 localTopSlotPosition {
		get {
			return new Vector3(0.0f, _backpackItems.Count + 1.0f, 0.0f);
		}
	}

	private List<BackpackItem> _backpackItems;

	private void Awake() {

		ObjectPool.CreatePool(_backPackItemPrefab, 10, transform);

		_backpackItems = new List<BackpackItem>(10);
	}

	public void PushBackpackItem(Tile tile) {

		var newBackpackItem = _backPackItemPrefab.Spawn();
		newBackpackItem.SetContent(tile);
		newBackpackItem.SetLocalPosition(localTopSlotPosition);
		_backpackItems.Add(newBackpackItem);
	}

	public Tile PopBackpackItem() {		

		if (_backpackItems.Count == 0) {
			return null;
		}
		else {
			var backpackItem = _backpackItems[_backpackItems.Count - 1];
			var tile = backpackItem.tile;
			_backpackItems.RemoveAt(_backpackItems.Count - 1);
			backpackItem.Recycle();
			return tile;
		}
	}
}
