using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialAmountOfAir;
    [SerializeField] private Vector2 _initialVelocity;

    private bool _isSimulationRunning = true;

}