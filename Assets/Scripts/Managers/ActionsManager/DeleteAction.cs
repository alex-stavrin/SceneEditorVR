using UnityEngine;
using System.Collections.Generic;

public class DeleteAction : UserAction
{
    List<GameObject> gameObjects;

    public DeleteAction(List<GameObject> newGameObjects)
    {
        gameObjects = newGameObjects;
    }

    public override void Do()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }

    public override void Undo()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }
}