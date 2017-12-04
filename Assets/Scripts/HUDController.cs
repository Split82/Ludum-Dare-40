using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class HUDController : MonoBehaviour {

	[SerializeField] PositionDirectionGameEvent _enemyWasHitEvent;
	[SerializeField] BasicGameEvent _counterDataDidChangeEvent;
	[SerializeField] BasicGameEvent _bossDidSpawnEvent;
	[SerializeField] IntVariable _bossKillsCountVariable;
	[SerializeField] IntVariable _pickedPickupsCountVariable;
	[SerializeField] IntVariable _bossEnergyVariable;	
	[Space]
	[SerializeField] Text _bossKillsText;
	[SerializeField] Text _crystalsText;
	[SerializeField] Text _bossEnergyText;

	private StringBuilder _stringBuilder;

	private void Awake() {

		_stringBuilder = new StringBuilder(150);
	}

	private void Start() {

		UpdateBossEnergyLabel();
		UpdateCounterLabels();
	}

	private void OnEnable() {

		_counterDataDidChangeEvent.Subscribe(HandleCounterDataDidChangeEvent);
		_enemyWasHitEvent.Subscribe(HandleEnemyWasHitEvent);
		_bossDidSpawnEvent.Subscribe(HandleBossDidSpawnEvent);
	}

	private void OnDisable() {

		_counterDataDidChangeEvent.Unsubscribe(HandleCounterDataDidChangeEvent);
		_enemyWasHitEvent.Unsubscribe(HandleEnemyWasHitEvent);
		_bossDidSpawnEvent.Unsubscribe(HandleBossDidSpawnEvent);
	}
	
	private void HandleCounterDataDidChangeEvent(object obj, GameEvent gameEvent) {		
		
		UpdateCounterLabels();
	}

	private void HandleEnemyWasHitEvent(object obj, GameEvent gameEvent) {

		UpdateBossEnergyLabel();
	}	

	private void HandleBossDidSpawnEvent(object obj, GameEvent gameEvent) {

		UpdateBossEnergyLabel();
	}	

	private void UpdateCounterLabels() {

		_crystalsText.text = "CRYSTALS: " + _pickedPickupsCountVariable.value;
		_bossKillsText.text = "BOSS KILLS: " + _bossKillsCountVariable.value;
	}

	private void UpdateBossEnergyLabel() {
		
		_stringBuilder.Length = 0;
		for (int i = 0; i < _bossEnergyVariable.value; i++) {
			_stringBuilder.Append('O');
		}
		_bossEnergyText.text = _stringBuilder.ToString();
	}
}