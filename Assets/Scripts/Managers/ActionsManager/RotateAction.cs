using System.Collections.Generic;
using UnityEngine;

public class RotateAction : UserAction
{
    private List<GameObject> gameObjects;
    private List<Quaternion> oldRotations;
    private List<Quaternion> newRotations;

    public RotateAction(List<GameObject> n_gameObjects, List<Quaternion> n_oldRotations, List<Quaternion> n_newRotations)
    {
        gameObjects = n_gameObjects;
        oldRotations = n_oldRotations;
        newRotations = n_newRotations;
    }

    public override void Do()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.rotation = newRotations[i];
        }
    }

    public override void Undo()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.rotation = oldRotations[i];
        }       
    }
}