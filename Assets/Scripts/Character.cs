using UnityEngine;

[System.Serializable]
public class Character
{
    public string characterName; // Name of the character
    public Sprite characterImage; // Image of the character
    public int trustLevel; // How much the character trusts the player
    public DialogueNode[] dialogueNodes; // All the dialogue for this character
}