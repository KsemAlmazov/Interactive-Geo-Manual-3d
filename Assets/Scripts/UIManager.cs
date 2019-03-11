using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject CountriesListPanel;
    public GameObject MainCamera;
    
    public void CountriesListButtonClick()
    {
        //Проверяем состояние камеры
        if (MainCamera.GetComponent<CameraManager>().CanMoveCamera == true)
        {
            //Заполняем таблицу с информацией о выбранных странах
            this.gameObject.GetComponent<MarkerTouchManager>().FillListWithSelectedCountries();

            //Здесь анимация появления панели
            CountriesListPanel.GetComponent<Animation>().Play("CountriesListSlide");

            //Отключаем возможность двигать камеру и взаимодействовать с тэгами на карте мира
            this.gameObject.GetComponent<MarkerTouchManager>().ActionIsPossible = false;
            MainCamera.GetComponent<CameraManager>().CanMoveCamera = false;
        }
        else
        {
            //Здесь анимация пропадания панели
            CountriesListPanel.GetComponent<Animation>().Play("CountriesListSlideBackwards");

            //Включаем возможность двигать камеру и взаимодействовать с тэгами на карте мира
            this.gameObject.GetComponent<MarkerTouchManager>().ActionIsPossible = true;
            MainCamera.GetComponent<CameraManager>().CanMoveCamera = true;
        }
    }
}
