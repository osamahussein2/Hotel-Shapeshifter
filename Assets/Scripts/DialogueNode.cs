using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public string dialogueText;
    public int nextNodeIndex;
    public List<Choice> choices;
}
