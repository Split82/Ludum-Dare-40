using UnityEngine;
using System.Collections;

public class RandomObjectPicker<T> {

	private T[] _objects;
	private float _lastPickTime;
	private float _minimumPickInterval;

    public RandomObjectPicker(T obj, float minimumPickInterval) {
        _objects = new T[1];
        _objects[0] = obj;
        _minimumPickInterval = minimumPickInterval;
    }

	public RandomObjectPicker(T[] objects, float minimumPickInterval) {        
		_objects = objects;
		_minimumPickInterval = minimumPickInterval;
	}

	public T PickRandomObject() {

		float nowTime = Time.timeSinceLevelLoad;
		if (nowTime - _lastPickTime < _minimumPickInterval) {
			return default(T);
		}

		_lastPickTime = nowTime;

        if (_objects.Length == 1) {
            return _objects[0];
        }
        else {
    		// Pick object. Don't use the last one.
    		int idx = Random.Range(0, _objects.Length - 1);
    		T pickedObject = _objects[idx];

    		// Move picked object to the end, so it is not picked next time.
    		_objects[idx] = _objects[_objects.Length - 1];
    		_objects[_objects.Length - 1] = pickedObject;

    		return pickedObject;
        }
	}
}
