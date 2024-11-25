using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; // Prefab of the button to create
    [SerializeField] private Transform gridParent;    // Parent object with GridLayoutGroup
    [SerializeField] private Sprite defaultThumbnail; // Default thumbnail if none is set

    // Scenes to exclude
    [SerializeField] private string[] excludedScenes = { "MainMenu", "LevelSelection", "Credits" };

    void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        int id=1;
        // Loop through all scenes in the build settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            // Skip excluded scenes
            if (System.Array.Exists(excludedScenes, name => name == sceneName))
                continue;

            // Create a button for the scene
            GameObject button = Instantiate(buttonPrefab, gridParent);

            // Set the button's text to the scene name
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = id++.ToString();
            }

            // Set a thumbnail if your button prefab supports it
            // Image buttonImage = button.GetComponentInChildren<Image>();
            // if (buttonImage != null && defaultThumbnail != null)
            // {
            //     buttonImage.sprite = defaultThumbnail;
            // }

            // Add an OnClick listener to load the scene
            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                string sceneToLoad = sceneName; // Avoid closure issue
                btn.onClick.AddListener(() => LoadLevel(sceneToLoad));
            }
        }
    }

    private void LoadLevel(string sceneName)
    {
        SettingsMenu.instance.PlayClip(1);
        SceneManager.LoadScene(sceneName);
    }
}
