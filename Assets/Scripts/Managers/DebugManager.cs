using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    [Header("State")]

    [SerializeField]
    bool showStates;

    [SerializeField]
    int maxActorsInScene = 100;

    [SerializeField]
    GameObject text3DPrefab;

    List<TextMeshPro> texts;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if(showStates)
        {            
            texts = new List<TextMeshPro>(100);
            for(int i = 0; i < maxActorsInScene; i++)
            {
                if(text3DPrefab)
                {                
                    GameObject textGameobject = Instantiate(text3DPrefab);
                    if(textGameobject)
                    {
                        TextMeshPro text = textGameobject.GetComponent<TextMeshPro>();
                        if(text)
                        {
                            texts.Add(text);
                        }
                    }
                    textGameobject.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if(showStates)
        {            
            List<Actor> actors = ActorsManager.GetActors();
            for(int i = 0; i < actors.Count; i++)
            {
                texts[i].gameObject.SetActive(true);
                texts[i].transform.position = actors[i].transform.position;
                texts[i].text = actors[i].GetInteractableState().ToString();
            }
        }
    }
}
