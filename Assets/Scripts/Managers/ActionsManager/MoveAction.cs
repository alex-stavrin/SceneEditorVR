using System.Collections.Generic;
using UnityEngine;

public class MoveAction : UserAction
{
    private List<GameObject> gameObjects;
    private List<Vector3> oldPositions;
    private List<Vector3> newPositions;

    public MoveAction(List<GameObject> n_gameObjects, List<Vector3> n_oldPositions, List<Vector3> n_newPositions)
    {
        gameObjects = n_gameObjects;
        oldPositions = n_oldPositions;
        newPositions = n_newPositions;
    }

    public override void Do()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.position = newPositions[i];
        }
    }

    public override void Undo()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.position = oldPositions[i];
        }       
    }
}