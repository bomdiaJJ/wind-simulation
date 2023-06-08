using UnityEngine;
using Sirenix.OdinInspector;

public class SimulationDebug : MonoBehaviour {
    [SerializeField] private AirSimulation _airSimulation;

    [SerializeField] private Vector2Int _selectedNode;
    [SerializeField] private float _amountOfAirToSet;

    [Button("Set amount of air to node by index")]
    public void SetAmountOfAirByIndex() {
        if (_selectedNode.x >= _airSimulation.Nodes.GetLength(0) || _selectedNode.y >= _airSimulation.Nodes.GetLength(1)) {
            Debug.Log("Node não existe (Indice fora do limite).");
            return;
        }

        _airSimulation.Nodes[_selectedNode.x, _selectedNode.y].AmountOfAir = _amountOfAirToSet;
    }
}