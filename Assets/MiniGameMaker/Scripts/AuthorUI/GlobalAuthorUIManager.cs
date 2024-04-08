using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalAuthorUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GlobalAuthorUIManager Instance;

    private List<SubAuthorUI> subAuthorUIs = new List<SubAuthorUI>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        subAuthorUIs = FindObjectsOfType<SubAuthorUI>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
