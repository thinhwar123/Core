using Sirenix.OdinInspector;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [field: SerializeField] public Camera MainCamera {get; private set;}
    [field: SerializeField] public Transform MainCameraTransform {get; private set;}
    [field: SerializeField] public Camera DefaultCamera {get; private set;}
    [field: SerializeField] public Camera FocusCamera {get; private set;}
    [field: SerializeField] public Transform CameraPivot {get; private set;}
    [field: SerializeField] public float SmoothTime {get; private set;}
    
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public float FocusZoom {get; private set;}
    [field: SerializeField,ReadOnly, FoldoutGroup("Debug")] public bool IsFocus {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public float DefaultZoom {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public float CurrentZoom {get; private set;}
    
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public float Ratio {get; private set;}
    
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Vector3 StartZoomPosition {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Vector3 CurrentZoomPosition {get; private set;}
    
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Vector3 DefaultPosition {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Vector3 CurrentPosition {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Vector3 TargetPosition {get; private set;}

    private Plane GroundPlane { get; set; }
    private float m_ZoomVelocity;
    private Vector3 m_MoveVelocity;
    private void Awake()    
    {
        GroundPlane = new Plane(Vector3.up, Vector3.zero);
        
        DefaultZoom = DefaultCamera.orthographicSize;
        FocusZoom = FocusCamera.orthographicSize;
        CurrentZoom = DefaultZoom;
        
        Ratio = Mathf.InverseLerp(DefaultZoom, FocusZoom, MainCamera.orthographicSize);
        
        DefaultPosition = CameraPivot.position;
        CurrentPosition = DefaultPosition;
    }

    public void Update()
    {
        UpdateZoomCamera();
        UpdateMoveCamera();
    }

    private void UpdateZoomCamera()
    {
        if (Mathf.Abs(MainCamera.orthographicSize - CurrentZoom) > 0.01f)
        {
            MainCamera.orthographicSize = Mathf.SmoothDamp(MainCamera.orthographicSize, CurrentZoom, ref m_ZoomVelocity, SmoothTime);
            Ratio = Mathf.InverseLerp(DefaultZoom, FocusZoom, MainCamera.orthographicSize);
        }
        else
        {
            MainCamera.orthographicSize = CurrentZoom;
            Ratio = Mathf.InverseLerp(DefaultZoom, FocusZoom, MainCamera.orthographicSize);
        }
    }
    public void SetFocusZoom()
    {
        CurrentZoom = FocusZoom;
        IsFocus = true;
    }
    public void CalculateTargetPosition()
    {
        StartZoomPosition = GetCurrentDefaultPosition();
        CurrentZoomPosition = GetCurrentFocusPosition();
        TargetPosition = DefaultPosition + StartZoomPosition - CurrentZoomPosition;
    }
    public void SetDefaultZoom()
    {
        CurrentZoom = DefaultZoom;
        IsFocus = false;
    }

    private Vector3 GetCurrentFocusPosition()
    {
        Ray ray = FocusCamera.ScreenPointToRay(Input.mousePosition);
        return GroundPlane.Raycast(ray, out float entry) ? ray.GetPoint(entry) : Vector3.zero;
    }
    private Vector3 GetCurrentDefaultPosition()
    {
        Ray ray = DefaultCamera.ScreenPointToRay(Input.mousePosition);
        return GroundPlane.Raycast(ray, out float entry) ? ray.GetPoint(entry) : Vector3.zero;
    }
    
    private void UpdateMoveCamera()
    {
        CameraPivot.position = Vector3.Lerp(DefaultPosition, TargetPosition, Ratio);
    }
}
