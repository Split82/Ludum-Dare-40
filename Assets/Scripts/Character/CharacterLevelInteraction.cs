using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterLevelInteraction : MonoBehaviour {

	[SerializeField] MainCameraProvider _mainCameraProvider;
	[Space]
	[SerializeField] CharacterMovementController _characterMovementController;	
	[SerializeField] CharacterBackpack _characterBackpack;
	[SerializeField] Tilemap _tilemap;

	//public event System.Action<Tile> didPickupTileEvent;		

	private Camera _camera;

	private void Start() {

		 _camera = _mainCameraProvider.camera;
	}

	public bool PositionIsInInteractionZone(Vector3Int position, Vector3Int characterPosition) {

		Vector3Int characterToDestination = position - characterPosition;

		// Can't spawn on itself.
		if (characterToDestination == Vector3Int.zero) {
			return false;
		}

		// Too far away.
		if (Mathf.Abs(characterToDestination.x) > 1 || Mathf.Abs(characterToDestination.y) > 1) {
			return false;
		}

		return true;	
	}

	private void Update() {

		if (_characterMovementController.grounded && Input.GetMouseButtonDown(0)) {

			var mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0.0f;
			Vector3Int position = _tilemap.WorldToCell(mouseWorldPos);	
			Vector3Int characterPosition = _tilemap.WorldToCell(_characterMovementController.position);

			// Interaction zone.
			if (!PositionIsInInteractionZone(position, characterPosition)) {
				return;
			}

			Vector3Int characterToDestination = position - characterPosition;

			var tile = _tilemap.GetTile(position) as Tile;

			// Put from backpack to map.
			if (tile == null) {
				// Check if we are not putting it above our head and we still have something in backpack.
				var backpackCollision = characterToDestination.x == 0 && characterToDestination.y <= _characterBackpack.numberOfItemsInBackpack + 1;				
				if (!backpackCollision) {
					var backpackTile = _characterBackpack.PopBackpackItem();
					if (backpackTile) {
						_tilemap.SetTile(position, backpackTile);
					}
				}
			}
			// Take from map to backpack.
			else {
				// Check collision between new backpack and map.
				if (_tilemap.GetTile(_tilemap.WorldToCell(_characterBackpack.topSlotPosition)) != null) {			
					return;
				}
				// Don't burry yourself.
				if (characterToDestination.x == 0 && characterToDestination.y == -1 && _tilemap.GetTile(characterPosition + Vector3Int.left) && _tilemap.GetTile(characterPosition + Vector3Int.right)) {
					return;
				}				

				_tilemap.SetTile(position, null);
				_characterBackpack.PushBackpackItem(tile);				
			}
		}
	}

}
