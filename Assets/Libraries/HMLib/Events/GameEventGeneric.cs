using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventGeneric<T> : GameEvent where T : GameEvent {

    private event System.Action<object, T> _gameEvent;   
    private event System.Action<object, GameEvent> _baseGameEvent;

    public void Subscribe(System.Action<object, T> foo) {
		_gameEvent += foo;        
    }

	public void Unsubscribe(System.Action<object, T> foo) {
		_gameEvent -= foo;
	}

    public override void Subscribe(System.Action<object, GameEvent> foo) {
        _baseGameEvent += foo;
    }
    
    public override void Unsubscribe(System.Action<object, GameEvent> foo) {
        _baseGameEvent -= foo;
    }

    public void Raise(Object sender, T gameEvent) {

        if (_gameEvent != null) {
            _gameEvent(sender, gameEvent);
        }

        if (_baseGameEvent != null) {
            _baseGameEvent(sender, gameEvent);
        } 
    }    

	public override void RaiseTest() {

		Raise(this, this as T);
	}
}
