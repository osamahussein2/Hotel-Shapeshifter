using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int dialogueProgress = 0; // Tracks progress of game

    public static bool didPlayerFinishSideQuest = false; // Make it so that the player can finish the side quest
}