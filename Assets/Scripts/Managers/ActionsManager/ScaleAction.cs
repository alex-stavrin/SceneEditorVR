using System.Collections.Generic;
using UnityEngine;

public class ScaleAction : UserAction
{
    private List<GameObject> gameObjects;
    private List<Vector3> oldScales;
    private List<Vector3> newScales;

    public ScaleAction(List<GameObject> n_gameObjects, List<Vector3> n_oldScales, List<Vector3> n_newScales)
    {
        gameObjects = n_gameObjects;
        oldScales = n_oldScales;
        newScales = n_newScales;
    }

    public override void Do()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.localScale = newScales[i];
        }
    }

    public override void Undo()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.localScale = oldScales[i];
        }       
    }
}