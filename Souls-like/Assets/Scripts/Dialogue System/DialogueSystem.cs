using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class DialogueSystem : MonoBehaviour
{
    DialogManager dialogManager;
    // Start is called before the first frame update
    void Start()
    {
        DialogData dialogData = new DialogData("/click/Enter name", "Me");

        dialogManager.Show(dialogData);
    }


}
