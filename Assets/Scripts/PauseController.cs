using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool _gamePaused;
    private static PauseController Instance { get ;  set; }
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

    public PauseController GetInstance() { return Instance; }
    
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gamePaused = !_gamePaused;
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = (!_gamePaused ? 0 : 1);
    }
}
