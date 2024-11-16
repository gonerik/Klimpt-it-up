using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool _gamePaused;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gamePaused = !_gamePaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = (!_gamePaused ? 0 : 1);
    }
}
