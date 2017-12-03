using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovementAI : MonoBehaviour {

	[SerializeField] TilemapProvider _tilemapProvider;	
	[Space]	
	[SerializeField] Tile _leftPassableTile;
	[SerializeField] Tile _rightPassableTile;
	[SerializeField] Rigidbody2D _rigidbody;
	[SerializeField] float _movementElasticity = 1.0f;
	[SerializeField] float _movementStrength = 1.0f;
	[SerializeField] float _movementFriction = 0.9f;
	[SerializeField] float _maxVelocity = 1.0f;
	[SerializeField] int _downDirection = -1;

	public MovingDirection movingDirection {
		get {
			return _movingDirection;
		}
	}

	public enum MovingDirection {
		Left,
		Right,
	}

	private Vector2 _velocity;
	private Vector2 _destination;
	private Vector3Int _prevCellPosition;

	private MovingDirection _movingDirection;

	private Tilemap _tilemap;
	
	public void Init(int downDirection, MovingDirection movingDirection) {

		_downDirection = downDirection;
		_movingDirection = movingDirection;
		_prevCellPosition.x = int.MaxValue;		
	}

	private void Awake() {

		_tilemap = _tilemapProvider.tilemap;
		_prevCellPosition.x = int.MaxValue;
		_movingDirection = MovingDirection.Left;
		_rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}	

	private void FixedUpdate() {

		Vector3 position = _rigidbody.position;

		Vector3Int cellPosition = _tilemap.WorldToCell(position);
		int xd = _movingDirection == MovingDirection.Left ? -1 : 1;
		if (cellPosition != _prevCellPosition) {
			var targetCellPosition = FindNewTargetPosition(cellPosition, xd);
			if (targetCellPosition.x < cellPosition.x) {
				_movingDirection = MovingDirection.Left;
			}
			else if (targetCellPosition.x > cellPosition.x) {
				_movingDirection = MovingDirection.Right;
			}
			_destination = _tilemap.CellToWorld(targetCellPosition) + _tilemap.cellSize * 0.5f;						
			_prevCellPosition = cellPosition;
		}

		_velocity += (_destination - (Vector2)position) * _movementElasticity * Time.fixedDeltaTime;
		_velocity *= _movementFriction;
		var sqrMagnitude = _velocity.sqrMagnitude;
		if (_velocity.sqrMagnitude > _maxVelocity * _maxVelocity) {
			_velocity = _maxVelocity * _velocity / Mathf.Sqrt(sqrMagnitude);
		}
		_rigidbody.MovePosition(position + (Vector3)_velocity * Time.fixedDeltaTime * _movementStrength);
	}

	private Vector3Int FindNewTargetPosition(Vector3Int cellPosition, int xd) {
		
		Vector3Int pr = cellPosition + new Vector3Int(xd, 0, 0);
		Vector3Int pl = cellPosition + new Vector3Int(-xd, 0, 0);
		Vector3Int pd = cellPosition + new Vector3Int(0, _downDirection, 0);
		
		bool gr = TileIsOccupied(pr, xd);
		bool gl = TileIsOccupied(pl, -xd);
		bool gd = TileIsOccupied(pd, xd);		

		// Enemy can't move any more.
		if (gr && gl && gd) {
			return cellPosition;
		}

		// Change moving direction.
		if (gr && gd) {	
			return FindNewTargetPosition(cellPosition, -xd);				
		}

		Vector3Int prd = cellPosition + new Vector3Int(xd, _downDirection, 0);
		bool grd = TileIsOccupied(prd, xd);

		// Can move right on ground.
		if (grd && !gr) {
			return pr;
		}

		if (grd && !gd) {
			return pd;
		}

		Vector3Int pr2 = cellPosition + new Vector3Int(xd * 2, 0, 0);
		Vector3Int pr2d = cellPosition + new Vector3Int(xd * 2, _downDirection, 0);

		bool gr2 = TileIsOccupied(pr2, xd);
		bool gr2d = TileIsOccupied(pr2d, xd);

		// Can move right flying over one empty cell.
		if (!gr && !gr2 && gr2d) {
			return pr;
		}

		Vector3Int pr3 = cellPosition + new Vector3Int(xd * 3, 0, 0);				
		Vector3Int pr3d = cellPosition + new Vector3Int(xd * 3, _downDirection, 0);
		
		bool gr3 = TileIsOccupied(pr3, xd);
		bool gr3d = TileIsOccupied(pr3d, xd);
		
		// Can move right flying over two obstacles.
		if (!gr && !gr2 && !gr3 && gr3d) {		
			return pr;
		}

		// Fly right down
		if (!gr && !gd && !grd) {		
			return prd;
		}

		return pr;
	}

	private bool TileIsOccupied(Vector3Int pos, int xd) {
		
		var tile = _tilemap.GetTile(pos);
		if (xd > 0) {
			return tile != null && tile != _rightPassableTile;
		}
		else if (xd < 0) {
			return tile != null && tile != _leftPassableTile;
		}
		else {
			return tile != null;
		}
	}

	private void OnDrawGizmosSelected() {

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(_destination, Vector3.one);
	}
}

