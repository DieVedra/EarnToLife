
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LayersProvider", menuName = "Providers/LayersProvider", order = 51)]

public class LayersProvider : ScriptableObject
{
        [SerializeField, Layer] private int _carLayer;
        [SerializeField, Layer] private int _zombieLayer;
        [SerializeField, Layer] private int _zombieDebrisLayer;
        
        [SerializeField, Layer] private int _layerDebris;
        
        
        // [SerializeField, Layer] private int _carLayer;
        // [SerializeField, Layer] private int _carLayer;

        public int CarLayer => _carLayer;
        public int ZombieLayer => _zombieLayer;
        public int ZombieDebrisLayer => _zombieDebrisLayer;
        public int LayerDebris => _layerDebris;
}