using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using uAdventure.Runner;

public class ButtonClicked : MonoBehaviour {

    private void Start()
    {
        InventoryManager.Instance.Show = false;
    }
    public void InitMenu()
    {
        Game.Instance.RunTarget("Menu", null);
    }
    public void InitMenuOptions() {
        Game.Instance.RunTarget("Menu_Options", null);
    }

    public void InitCreditos()
    {
        Game.Instance.RunTarget("Creditos", null);
    }

    public void InitMaleta()
    {
        Game.Instance.RunTarget("Maleta", null);
    }

    public void InitPapers()
    {
        Game.Instance.RunTarget("PapersPleaseFermin", null);
    }

    public void InitCuestionario()
    {
        Game.Instance.RunTarget("test1", null);
    }
}
