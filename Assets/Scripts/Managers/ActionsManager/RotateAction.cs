using System.Collections.Generic;
using UnityEngine;

public class RotateAction : UserAction
{
    private List<GameObject> gameObjects;
    private List<Quaternion> oldRotations;
    private List<Quaternion> newRotations;
    private List<Vector3> oldPositions;
    private List<Vector3> newPositions;

    public RotateAction(List<GameObject> n_gameObjects, List<Quaternion> n_oldRotations, List<Quaternion> n_newRotations, 
        List<Vector3> n_oldPositions, List<Vector3> n_newPositions)
    {
        gameObjects = n_gameObjects;
        oldRotations = n_oldRotations;
        newRotations = n_newRotations;
        oldPositions = n_oldPositions;
        newPositions = n_newPositions;
    }

    public override void Do()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.rotation = newRotations[i];
            gameObjects[i].transform.position = newPositions[i];
        }
    }

    public override void Undo()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.rotation = oldRotations[i];
            gameObjects[i].transform.position = oldPositions[i];
        }       
    }
}