using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class SimulationDebug : MonoBehaviour {
    [SerializeField] private AirSimulation _airSimulation;

    [SerializeField] private Vector2Int _selectedNodeIndex;
    [SerializeField] private float _amountOfAirToSet;

    [SerializeField] private TextMeshProUGUI _nodePropertiesText;

    private Camera _mainCamera;

    private void Start() {
        _mainCamera = Camera.main;
    }

    private void Update() {
        ShowNodeProperties();

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePositionInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePositionInWorld.x = Mathf.Round(mousePositionInWorld.x);
            mousePositionInWorld.y = Mathf.Round(mousePositionInWorld.y);
            mousePositionInWorld.z = 0;

            _selectedNodeIndex = new Vector2Int((int)mousePositionInWorld.x, (int)mousePositionInWorld.y);
        }
    }

    private void ShowNodeProperties() {
        // if (_selectedNodeIndex.x < 0 || _selectedNodeIndex.y < 0 || _selectedNodeIndex.x >= _airSimulation.Nodes.GetLength(0) || _selectedNodeIndex.y >= _airSimulation.Nodes.GetLength(1)) {
        //     _nodePropertiesText.text = "";
        //     return;
        // }

        // _nodePropertiesText.text = 
        //     string.Concat(
        //         $"Node [{_selectedNodeIndex.x}, {_selectedNodeIndex.y}]",
        //         "\nDensity: ", _airSimulation.Nodes[_selectedNodeIndex.x, _selectedNodeIndex.y].Density,
        //         "\nMagnitude: ", _airSimulation.Nodes[_selectedNodeIndex.x, _selectedNodeIndex.y].NodeVelocity.magnitude,
        //         "\nVelocity: ", _airSimulation.Nodes[_selectedNodeIndex.x, _selectedNodeIndex.y].NodeVelocity
        //     ); 
    }

    [Button("Set amount of air to node by index")]
    public void SetAmountOfAirByIndex() {
        if (_selectedNodeIndex.x < 0 || _selectedNodeIndex.y < 0) {
            Debug.Log("Node inválido (Índice negativo).");
            return;
        }

        if (_selectedNodeIndex.x >= _airSimulation.GridSize || _selectedNodeIndex.y >= _airSimulation.GridSize) {
            Debug.Log("Node não existe (Índice fora do limite).");
            return;
        }

        int nodeIndex = _airSimulation.IX(_selectedNodeIndex.x, _selectedNodeIndex.y);
        _airSimulation.Nodes[nodeIndex].Density = _amountOfAirToSet;
    }
}