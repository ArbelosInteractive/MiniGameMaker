using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalMinigameUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GlobalMinigameUIManager Instance;

    private List<SubMinigameUI> subMinigameUIs = new List<SubMinigameUI>();

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
        subMinigameUIs = FindObjectsOfType<SubMinigameUI>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
