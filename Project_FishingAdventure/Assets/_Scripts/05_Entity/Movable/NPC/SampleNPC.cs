using UnityEngine;

public class SampleNPC : NPC
{
    public override void Interact()
    {
        PrintText("this is sample npc");
    }    

    private void PrintText(string text){}
}
