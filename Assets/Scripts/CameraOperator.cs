using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraOperator : MonoBehaviour
{
    public static CameraOperator Instance { get; private set; }

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
    
    public float GetCameraSpeed() { return cameraSpeed; }
    public float GetZoomSpeed() { return zoomSpeed; }
    public float GetSize() { return size; }
    public void SetCameraSpeed(float value) { cameraSpeed = value; }
    public void SetZoomSpeed(float value) { zoomSpeed = value; }
    public void SetSize(float value) { size = value; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
