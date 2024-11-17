using TMPro;
using UnityEngine;

public class ShineOnMouse : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    // Start is called before the first frame update
    public void OnMouseEnter()
    {
        _textMesh.color = new Color(243/255f, 220/255f, 114/255f);
    }

    public void OnMouseExit()
    {
        _textMesh.color = new Color(58/255f, 45/255f, 63/255f);
    }
}
