using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterLevelInteraction : MonoBehaviour {

	[SerializeField] MainCameraProvider _mainCameraProvider;
	[Space]
	[SerializeField] CharacterMovementController _characterMovementController;	

	public event System.Action<Tile> didPickupTileEvent;

	[SerializeField] Tilemap _tilemap;
	[SerializeField] Tile _basicTile;

	private Camera _camera;

	private void Start() {

		 _camera = _mainCameraProvider.camera;
	}

	private void Update() {

		if (_characterMovementController.grounded && Input.GetMouseButtonDown(0)) {

			var mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0.0f;
			Vector3Int position = _tilemap.WorldToCell(mouseWorldPos);

			var tile = _tilemap.GetTile(position) as Tile;
			if (tile == null) {
				_tilemap.SetTile(position, _basicTile);
			}
			else {				
				_tilemap.SetTile(position, null);
				if (didPickupTileEvent != null) {
					didPickupTileEvent(tile);
				}
			}
		}
	}

}
