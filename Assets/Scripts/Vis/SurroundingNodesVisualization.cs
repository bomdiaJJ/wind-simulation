using UnityEngine;
using Shapes;

[ExecuteAlways]
public class SurroundingNodesVisualization : ImmediateModeShapeDrawer {
    [SerializeField] private float _pointDistance = 2f;
    [SerializeField] private float _pointDiscRadius = .4f;
    [SerializeField] private float _lineMinThickness = .1f;
    [SerializeField] private float _lineMaxThickness = .5f;

    private Camera _mainCamera;
    private Vector3 _mousePositionInWorld;

    private float _distance00 = 0f;
    private float _distance01 = 0f;
    private float _distance10 = 0f;
    private float _distance11 = 0f;

    private void Start() {
        _mainCamera = Camera.main;
    }

    public override void DrawShapes(Camera cam) {
        using(Draw.Command(cam)) {
            if (_mainCamera == null) return;

            _mousePositionInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mousePositionInWorld.x = Mathf.Clamp(_mousePositionInWorld.x, 0f, _pointDistance);
            _mousePositionInWorld.y = Mathf.Clamp(_mousePositionInWorld.y, 0f, _pointDistance);
            _mousePositionInWorld.z = 0;

            float lineThickness = Remap((Vector3.zero - _mousePositionInWorld).magnitude, 0f, Mathf.Sqrt(2) * _pointDistance, _lineMaxThickness, _lineMinThickness);
            Draw.Line(_mousePositionInWorld, Vector2.zero, lineThickness, new Color(1f, 1f, 1f, lineThickness));
            _distance00 = (Vector3.zero - _mousePositionInWorld).magnitude;

            lineThickness = Remap((Vector3.right * _pointDistance - _mousePositionInWorld).magnitude, 0f, Mathf.Sqrt(2) * _pointDistance, _lineMaxThickness, _lineMinThickness);
            Draw.Line(_mousePositionInWorld, Vector2.right * _pointDistance, lineThickness, new Color(1f, 1f, 1f, lineThickness));
            _distance10 = (Vector3.right * _pointDistance - _mousePositionInWorld).magnitude;
            
            lineThickness = Remap((Vector3.up * _pointDistance  - _mousePositionInWorld).magnitude, 0f, Mathf.Sqrt(2) * _pointDistance, _lineMaxThickness, _lineMinThickness);
            Draw.Line(_mousePositionInWorld, Vector2.up * _pointDistance, lineThickness, new Color(1f, 1f, 1f, lineThickness));
            _distance01 = (Vector3.up * _pointDistance  - _mousePositionInWorld).magnitude;
            
            lineThickness = Remap((new Vector3(1f, 1f) * _pointDistance  - _mousePositionInWorld).magnitude, 0f, Mathf.Sqrt(2) * _pointDistance, _lineMaxThickness, _lineMinThickness);
            Draw.Line(_mousePositionInWorld, Vector2.one * _pointDistance, lineThickness, new Color(1f, 1f, 1f, lineThickness));
            _distance11 = (new Vector3(1f, 1f) * _pointDistance  - _mousePositionInWorld).magnitude;

            Draw.Radius = _pointDiscRadius;
            Draw.Disc(Vector2.zero, Color.blue);
            Draw.Disc(Vector2.right * _pointDistance, Color.magenta);
            Draw.Disc(Vector2.up * _pointDistance, Color.cyan);
            Draw.Disc(Vector2.one * _pointDistance, Color.white);

            Color mousePointColor = new Color(
                Mathf.InverseLerp(0f, _pointDistance, _mousePositionInWorld.x),
                Mathf.InverseLerp(0f, _pointDistance, _mousePositionInWorld.y),
                1f
            );

            Draw.Disc(_mousePositionInWorld, mousePointColor);

            Draw.Text(Vector2.zero - new Vector2(.5f, .5f), _distance00.ToString("0.00"), 3f);
            Draw.Text((Vector2.right * _pointDistance) + new Vector2(.5f, -.5f), _distance10.ToString("0.00"), 3f);
            Draw.Text((Vector2.up * _pointDistance) + new Vector2(-.5f, .5f), _distance01.ToString("0.00"), 3f);
            Draw.Text((Vector2.one * _pointDistance) + new Vector2(.5f, .5f), _distance11.ToString("0.00"), 3f);

            float totalDistance = _distance00 + _distance01 + _distance10 + _distance11;
            Draw.Text((Vector2.up * (_pointDistance / 2f)) + new Vector2(_pointDistance / 2f, 0f), totalDistance.ToString("0.00"), 3f);
        }
    }

    public float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}