using UnityEngine;

public class CameraOperator : MonoBehaviour
{
    public static CameraOperator Instance { get;  set; }

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    [SerializeField] private float cameraSpeed = 1.5f;
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float size = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    [SerializeField] private bool enableBoundaries;
    [SerializeField] private bool enableMovement;
    
    /// <summary>
    /// The following variables are used to set up camera boundaries for each stage, limiting its movement
    /// </summary>
    
    [SerializeField] private float minXBoundary;
    [SerializeField] private float maxXBoundary;
    [SerializeField] private float minYBoundary;
    [SerializeField] private float maxYBoundary;
    public float GetCameraSpeed() { return cameraSpeed; }
    public float GetZoomSpeed() { return zoomSpeed; }
    public float GetSize() { return size; }
    public void SetCameraSpeed(float value) { cameraSpeed = value; }
    public void SetZoomSpeed(float value) { zoomSpeed = value; }
    public void SetSize(float value) { size = value; }
    public float GetMinXBoundary(){ return minXBoundary; }
    public void SetMinXBoundary(float f){ minXBoundary = f; }
    public float GetMinYBoundary(){ return minYBoundary; }
    public void SetMinYBoundary(float f){ minYBoundary = f; }
    public float GetMaxXBoundary(){ return maxXBoundary; }
    public void SetMaxXBoundary(float f){ maxXBoundary = f; }
    public float GetMaxYBoundary(){ return maxYBoundary; }
    public void SetMaxYBoundary(float f){ maxYBoundary = f; }
    
    // void Start()
    // {
    //     
    // }
    public void ResizeCamera(float newSize){
        cam.orthographicSize = Mathf.SmoothStep(cam.orthographicSize, newSize, zoomSpeed * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if (enableMovement)
        {
            float x = player.position.x;
            float y = player.position.y;
            Vector3 newCameraPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
            if (enableBoundaries)
            {
                if (x < minXBoundary) newCameraPosition.x = minXBoundary;
                else if (x > maxXBoundary) newCameraPosition.x = maxXBoundary;
                else newCameraPosition.x = x;
                if (y < minYBoundary) newCameraPosition.y = minYBoundary;
                else if (y > maxYBoundary) newCameraPosition.y = maxYBoundary;
                else newCameraPosition.y = y;
            }
            else
            {
                newCameraPosition.x = x;
                newCameraPosition.y = y;
            }
            transform.position = 
                Vector3.SlerpUnclamped(transform.position, newCameraPosition, cameraSpeed * Time.fixedDeltaTime);
        }
        ResizeCamera(size);
    }
}
