using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour {

	[EventSender] [SerializeField] BasicGameEvent _gameOverEvent;
	[EventSender] [SerializeField] BasicGameEvent _countersDidChangeEvent;
	[SerializeField] IntVariable _bossKillsCountVariable;
	[SerializeField] IntVariable _pickedPickupsCountVariable;
	[Space]
	[SerializeField] Vector3Variable _characterPosition;
	[SerializeField] PositionGameEvent _playerDidDieEvent;
	[SerializeField] PositionGameEvent _pickupWasPickedUpEvent;
	[SerializeField] PositionDirectionGameEvent _basicEnemyWasKilledEvent;
	[SerializeField] PositionDirectionGameEvent _bossWasKilledEvent;	
	[SerializeField] BasicGameEvent _bossDidHitRecycleTileEvent;
	[SerializeField] Ease01 _fadeEffectEase;
	[Space]	
	[SerializeField] Transform[] _pickupSpawnPlaces;
	[SerializeField] Pickup _pickupPrefab;
	[SerializeField] EnemySpawnPlace[] _basicEnemySpawnPlaces;
	[SerializeField] EnemySpawnPlace[] _bossSpawnPlaces;

	private enum GameState {
		Intro,
		WaitingForPickup,
		KillingBoss,	
		GameOver,	
	}

	private GameState gameState {
		set {
			if (_gameState == value || _gameState == GameState.GameOver) {
				return;
			}

			_gameState = value;
			switch (_gameState) {
				
				case GameState.WaitingForPickup: {
					SpawnPickup();
					break;
				}

				case GameState.KillingBoss: {
					SpawnBoss();
					break;
				}

				case GameState.GameOver: {
					_gameOverEvent.Raise(this, _gameOverEvent);
					_fadeEffectEase.FadeOut();
					break;
				}
			}
		}
	}

	private GameState _gameState;
	private int _pickedPickupsCount;
	private int _bossKillsCount;
	private float _nextBasicEnemyTimer;

	private void Start() {

		_pickupPrefab.CreatePool(1);
		gameState = GameState.WaitingForPickup;
		_bossKillsCountVariable.value = 0;
		_pickedPickupsCountVariable.value = 0;
	}

	private void OnEnable() {

		_pickupWasPickedUpEvent.Subscribe(HandlePickupWasPickedUpEvent);
		_basicEnemyWasKilledEvent.Subscribe(HandleBasicEnemyWasKilledEvent);
		_bossWasKilledEvent.Subscribe(HandleBossWasKilledEvent);
		_playerDidDieEvent.Subscribe(HandlePlayerDidDieEvent);
		_bossDidHitRecycleTileEvent.Subscribe(HandleBossDidHitRecycleTileEvent);
	}

	private void OnDisable() {

		_pickupWasPickedUpEvent.Unsubscribe(HandlePickupWasPickedUpEvent);
		_basicEnemyWasKilledEvent.Unsubscribe(HandleBasicEnemyWasKilledEvent);
		_bossWasKilledEvent.Unsubscribe(HandleBossWasKilledEvent);
		_playerDidDieEvent.Unsubscribe(HandlePlayerDidDieEvent);
		_bossDidHitRecycleTileEvent.Unsubscribe(HandleBossDidHitRecycleTileEvent);
	}

	private void Update() {
		
		_nextBasicEnemyTimer -= Time.deltaTime;
		if (_nextBasicEnemyTimer <= 0.0f) {
			SpawnBasicEnemy();
			_nextBasicEnemyTimer = 4.0f / (_pickedPickupsCount * 0.2f + 1.0f);
		}		
	}

	private void SpawnPickup() {
		
		Vector3 furthestPickupSpawnPlacePosition = Vector3.zero;
		float maxSqrDistance = 0.0f;
		foreach (var pickupSpawnPlace in _pickupSpawnPlaces) {
			Vector3 pos = pickupSpawnPlace.position;
			float sqrDistance = (pos - _characterPosition.value).sqrMagnitude;
			if (sqrDistance > maxSqrDistance) {
				maxSqrDistance = sqrDistance;
				furthestPickupSpawnPlacePosition = pos;
			}
		}
		
		_pickupPrefab.Spawn(furthestPickupSpawnPlacePosition);
	}

	private void SpawnBoss() {

		var bossSpawnPlace = _bossSpawnPlaces[Random.Range(0, _bossSpawnPlaces.Length)];
		bossSpawnPlace.SpawnEnemy(4 + 2 * _pickedPickupsCount);
	}

	private void SpawnBasicEnemy() {

		var spawnPlace = _basicEnemySpawnPlaces[Random.Range(0, _basicEnemySpawnPlaces.Length)];
		spawnPlace.SpawnEnemy(1);
	}

	private void HandlePickupWasPickedUpEvent(object obj, GameEvent gameEvent) {

		_pickedPickupsCount++;
		_pickedPickupsCountVariable.value = _pickedPickupsCount;
		_countersDidChangeEvent.Raise(this, _countersDidChangeEvent);
		gameState = GameState.KillingBoss;
	}
	
	private void HandleBossWasKilledEvent(object obj, GameEvent gameEvent) {

		_bossKillsCount++;
		_bossKillsCountVariable.value = _bossKillsCount;
		_countersDidChangeEvent.Raise(this, _countersDidChangeEvent);
		gameState = GameState.WaitingForPickup;
	}

	private void HandlePlayerDidDieEvent(object obj, GameEvent gameEvent) {

		ScoreManager.PushScore(_bossKillsCount);
		gameState = GameState.GameOver;
		StartCoroutine(GoToMainMenuAfterDelayCoroutin(1.0f));
	}

	private void HandleBossDidHitRecycleTileEvent(object obj, GameEvent gameEvent) {

		gameState = GameState.WaitingForPickup;
	}

	private void HandleBasicEnemyWasKilledEvent(object obj, GameEvent gameEvent) {
		
	}

	private IEnumerator GoToMainMenuAfterDelayCoroutin(float duration) {

		MainMenu.showScoreBar = true;
		yield return new WaitForSeconds(duration);
		SceneManager.LoadScene("MainMenu");
	}
}
