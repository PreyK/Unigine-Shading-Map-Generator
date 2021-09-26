using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GithubLink : MonoBehaviour
{
    public string url = "https://github.com/PreyK/Unigine-Shading-Map-Generator";
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            
            Application.OpenURL(url);
        });
    }
}
