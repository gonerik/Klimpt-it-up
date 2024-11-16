using System.Collections;
using UnityEngine;

public class MopSign : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine("Dispose", 0f);
    }

    private IEnumerator Dispose(){
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
