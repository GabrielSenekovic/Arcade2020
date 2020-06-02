using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public void InitiateTutorial(EntityManager entityManager, UIManager UI, Team team, Room currentRoom, Vector2 RoomSize)
    {
        team.players[0].GetComponent<Movement>().ToggleFrozen(true);
        team.players[1].GetComponent<Movement>().ToggleFrozen(true);
        GameObject newEnemy = Instantiate(entityManager.TypesOfEnemies[0], new Vector2(RoomSize.x/2, RoomSize.y/2), Quaternion.identity, currentRoom.transform);
        entityManager.entities.Add(newEnemy.GetComponent<Movement>());
        entityManager.amountOfEnemiesSpawned++;
        UI.minimap.gameObject.SetActive(false);
        currentRoom.roomCleared = false;
        foreach(GameObject door in currentRoom.doors)
        {
            door.GetComponent<Door>().OpenClose(false);
        }
        entityManager.battleInitiated = true;
        newEnemy.GetComponent<EnemyController>().Spawn();
    }
    public bool OnPlay(EntityManager entityManager, UIManager UI, Manuscript.Dialog dialog, Team team)
    {
        if(entityManager.entities.Count > 2)
        {
            if(!entityManager.entities[2].GetComponent<EnemyController>().isSpawning &&UI.speechBubble_Obj.lineIndex == 0)
            {
                entityManager.entities[2].ToggleFrozen(true);
                UI.OpenOrClose(UI.speechBubble);
                UI.speechBubble_Obj.Say(dialog.myLines[0]);
            }
        }
        else if(UI.speechBubble_Obj.lineIndex == 1 && team.players[1].GetComponent<PlayerBallController>().balls.Count == 1)
        {
            UI.OpenOrClose(UI.speechBubble);
            UI.speechBubble_Obj.Say(dialog, 2);
        }
        else if(UI.speechBubble_Obj.lineIndex == 3 && team.players[0].GetComponent<PlayerBallController>().balls.Count == 3)
        {
            UI.OpenOrClose(UI.speechBubble);
            UI.speechBubble_Obj.Say(dialog.myLines[3]);
        }
        else if(UI.speechBubble_Obj.lineIndex == 4 && team.players[0].GetComponent<PlayerMovementController>().HasDashed())
        {
            UI.OpenOrClose(UI.speechBubble);
            UI.speechBubble_Obj.Say(dialog.myLines[4]);
        }
        else if(UI.speechBubble_Obj.lineIndex == 5 && team.players[1].GetComponent<PlayerMovementController>().HasDashed())
        {
            team.players[0].GetComponent<Movement>().ToggleFrozen(false);
            team.players[1].GetComponent<Movement>().ToggleFrozen(false);
            UI.OpenOrClose(UI.speechBubble);
            UI.speechBubble_Obj.Say(dialog, 4);
            return false;
        }
        return true;
    }
}
