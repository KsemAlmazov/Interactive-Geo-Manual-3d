using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour {

    //Содержание класса Страна
    [System.Serializable]
    public class CountryInfo
    {
        public string CountryName;
        public int TerritorySize;
        public float GDPSize;
        public int PopulationSize;
        public GameObject CountryModel;
        public bool IsSelected;
    }

    [SerializeField]
    public CountryInfo[] CountryList;
}
