using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;

public class PlayerPrefsTools : MonoBehaviour {
    
    [MenuItem ("Tools/Reset Player Prefs")]
    private static void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}