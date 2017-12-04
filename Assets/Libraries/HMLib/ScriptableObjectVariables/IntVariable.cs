using UnityEngine;


[CreateAssetMenu(fileName = "IntVariable", menuName = "HMLib/IntVariable")]
public class IntVariable : ScriptableObject {

    [SerializeField] int _defaultValue;
    public int value;

    private void OnEnable() {

        value = _defaultValue;
    }
}
