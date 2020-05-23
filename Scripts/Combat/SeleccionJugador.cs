using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Esta clase se encarga de coger el nombre del campeon y pasarlo a la clase netmanage
 */

public class SeleccionJugador : MonoBehaviour
{
    public Text CampeonNombre;

    public void SetChampionName(string _name)
    {
        this.CampeonNombre.text = _name;

    }
}
