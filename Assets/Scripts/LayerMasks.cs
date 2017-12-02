using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LayerMasks {

    public static LayerMask groundLayerMask {
        get {
            if (_groundLayerMask.value == 0) {
                _groundLayerMask = GetLayerMask("Ground");
            }                
            return _groundLayerMask;
        }
    }
    private static LayerMask _groundLayerMask;

    private static LayerMask GetLayerMask(string layerName) {
        
        int layerNum = LayerMask.NameToLayer(layerName);
        Assert.IsTrue(layerNum > 0);
        LayerMask layerMask = new LayerMask();
        layerMask.value = 1 << layerNum;
        return layerMask;
    }
}
