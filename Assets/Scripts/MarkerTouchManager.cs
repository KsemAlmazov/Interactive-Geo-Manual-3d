using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerTouchManager : MonoBehaviour {

    public Transform CountryObject;
    public Transform GRID;

    public GameObject CountryInfoPanel;
    public GameObject CountryAmountPanel;
    public GameObject ClearListButton;
    public GameObject CountryHorizontalInfoPrefab;

    public Text TerritorySizeText;
    public Text GDPText;
    public Text PopulationSizeText;
    public Text SelectedCountriesNumber;

    public bool ActionIsPossible = true;
    public int SelectedListLength;

    private float translation;

    private bool IsTapped = false;
    private bool ActionRegistered;


    void Update()
    {   
        //Зажали кнопку/тач
        if (Input.GetMouseButton(0) && ActionRegistered == false && ActionIsPossible == true) 
        {
            //Debug.Log("Clicked Successfully");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                //Debug.Log("Ray Created");
                
                //Transform HitObject = hit.transform;

                //Debug.Log("Ray hit : " + hit.transform.name);

                CountryObject = hit.transform;

                //Если попали в коллайдер геомаркера лучом из камеры, то считаем время зажатия
                if (hit.transform.tag == "CountryColliderBody")
                {
                    //Считаем время зажатия вместо кадров
                    translation += Time.deltaTime;
                }

                //Здесь происходит действие при зажатии маркера, если держать палец в течении секунды
                if (translation >= 1)
                {

                    ActionRegistered = true;
                    //Debug.Log("Registered Hold, name : " + CountryObject.name);

                    int TempInt = System.Int32.Parse(SelectedCountriesNumber.text);

                    //Обнулили тапнутую страну
                    if (TempInt == 0) ClearListAction();

                    //Проверяем, если страна была заранее тапнута, то оставить 3д модель и текст без анимаций
                    if (IsTapped == true)
                    {
                        CountryObject.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                        CountryObject.transform.GetChild(1).localScale = new Vector3(1, 1, 1);
                    }

                    //Добавили +1 к тексту о количестве выбранных стран, если страна ещё не была выбрана
                    if (this.gameObject.GetComponent<CountryManager>().CountryList[CountryObject.GetComponent<MarkerClickDetector>().CountyID].IsSelected == false)
                        SelectedCountriesNumber.text = (TempInt + 1).ToString();

                    //Если статус маркера = был тапнут, то проигрываем анимацию появления панели с количеством, выделенных стран
                    if (CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState == 1)
                    {
                        CountryInfoPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimationBackwards");
                    }

                    //Поменяли геометку на иконку галочки
                    CountryObject.transform.GetChild(2).localScale = new Vector3(0, 0, 0);
                    CountryObject.transform.GetChild(3).localScale = new Vector3(1, 1, 1);
                    //Галочку сделали зеленой, показывая что страна выбрана
                    CountryObject.transform.GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.green);

                    //Указали в глобальном списке стран, о том, что страна выделена
                    this.gameObject.GetComponent<CountryManager>().CountryList[CountryObject.GetComponent<MarkerClickDetector>().CountyID].IsSelected = true;

                    //Проверяем если страна зажата впервые
                    if (TempInt == 0)
                    {
                        //Отображаем кнопку "Очистить список стран"
                        ClearListButton.GetComponent<Animation>().Play("CountryInfoSlideAnimation");
                        CountryAmountPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimation");
                    }

                    //Объявили о маркере, что он был зажат
                    CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState = 2;

                    //Проверяем наличие 3д модели и названия страны, если их не видно - проигрываем анимацию
                    if (CountryObject.transform.GetChild(0).localScale == new Vector3(0, 1, 1) && IsTapped == false)
                    {
                        CountryObject.transform.GetChild(0).GetComponent<Animation>().Play();
                    }
                    if (CountryObject.transform.GetChild(1).localScale == new Vector3(0, 0, 0) && IsTapped == false)
                    {
                        CountryObject.transform.GetChild(1).GetComponent<Animation>().Play();
                    }

                    translation = 0;
                    ActionRegistered = false;
                    IsTapped = false;
                }

            }
        }
        //Проверка поднят ли палец/кнопка мыши
        else if (Input.GetMouseButtonUp(0) && ActionIsPossible == true)
        {
            //Здесь происходит проверка и действие при тапе маркера
            if (translation > 0.05 && translation < 1 && CountryObject.tag == "CountryColliderBody" && ActionRegistered == false &&
                CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState == 0)
            {
                //Debug.Log("Registered Tap, name : " + CountryObject.name);

                //Очистили выделение всех стран т.к. у нас единичный тап - выделена только одна страна
                ClearListAction();

                //Зарегестрировали действие, чтобы луч при нажатии по экрану временно не читался
                ActionRegistered = true;
                IsTapped = true;
                //Проигрываем анимацию интерфейса в зависимости от состояния иконки (геотэга/галочки)
                if (CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState == 0)
                    CountryInfoPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimation");
                if (CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState == 2)
                    CountryAmountPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimationBackwards");

                //Объявили о маркере, что он тапнут
                CountryObject.GetComponent<MarkerClickDetector>().CurrentMarkerState = 1;

                //Проиграли анимацию названия
                Transform CountryNamePointer = CountryObject.transform.GetChild(0);
                CountryNamePointer.GetComponent<Animation>().Play();

                //Поменяли геометку на иконку галочки
                CountryObject.transform.GetChild(2).localScale = new Vector3(0, 0, 0);
                CountryObject.transform.GetChild(3).localScale = new Vector3(1, 1, 1);

                //Получаем ID из префаба страны
                int ChoosedCountryID = CountryObject.GetComponent<MarkerClickDetector>().CountyID;
                //Обращаемся по ID в префабе страны к информации в массиве-списке стран
                this.gameObject.GetComponent<CountryManager>().CountryList[ChoosedCountryID].CountryModel.GetComponent<Animation>().Play();

                //Далее получаем и заполняем информацию о выбранной стране для панели информации
                int TerritorySize  = this.gameObject.GetComponent<CountryManager>().CountryList[ChoosedCountryID].TerritorySize;
                float GDPSize      = this.gameObject.GetComponent<CountryManager>().CountryList[ChoosedCountryID].GDPSize;
                int PopulationSize = this.gameObject.GetComponent<CountryManager>().CountryList[ChoosedCountryID].PopulationSize;

                //Заполняем информацию в панели информации о стране
                TerritorySizeText.text = "Площадь : " + TerritorySize.ToString() + " КМ2";
                GDPText.text = "ВВП : " + GDPSize.ToString() + " трлн.долл.";
                PopulationSizeText.text = "Население : " + PopulationSize.ToString();

                //Отпустили действие - теперь взаимодействие с экраном может считаеться заново
                ActionRegistered = false;
            }
            translation = 0;
        }

    }

    //Очистка выделения всех стран и возврат к оригинальным параметрам
    public void ClearListAction()
    {
        //Отыскали все текущие страны с интерактивными маркерами на карте
        GameObject[] WorldMapCountries = GameObject.FindGameObjectsWithTag("CountryColliderBody");

        //Обнулили UI число количества выделенных стран
        SelectedCountriesNumber.text = "0";

        //Обращаемся к каждому отдельному префабу
        foreach (GameObject WorldMapCountry in WorldMapCountries)
        {
            //Вернули графические параметры дочерних объектов к изначальным
            WorldMapCountry.transform.GetChild(0).localScale = new Vector3(0, 1, 1);
            WorldMapCountry.transform.GetChild(1).localScale = new Vector3(0, 0, 0);
            WorldMapCountry.transform.GetChild(2).localScale = new Vector3(1, 1, 1);
            WorldMapCountry.transform.GetChild(3).localScale = new Vector3(0, 0, 0);

            WorldMapCountry.transform.GetChild(0).GetComponent<Animation>().Stop();
            WorldMapCountry.transform.GetChild(1).GetComponent<Animation>().Stop();

            //Вернули галочке серый цвет
            WorldMapCountry.transform.GetChild(3).GetComponent<Renderer>().material.SetColor("_Color", Color.white);

            //Проиграли анимации интерфейса исходя из текущего состояния маркеров
            if (WorldMapCountry.GetComponent<MarkerClickDetector>().CurrentMarkerState == 2)
            {
                ClearListButton.GetComponent<Animation>().Play("CountryInfoSlideAnimationBackwards");
                CountryAmountPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimationBackwards");
            }else if (WorldMapCountry.GetComponent<MarkerClickDetector>().CurrentMarkerState == 1)
            {
                CountryInfoPanel.GetComponent<Animation>().Play("CountryInfoSlideAnimationBackwards");
            }
            //Обнулили состояние маркера т.е. геомаркер не нажат и страна не выбрана (для локального скрипта)
            WorldMapCountry.GetComponent<MarkerClickDetector>().CurrentMarkerState = 0;

            //Указали в Списке стран, что она не выбрана (для глобального массива со странами)
            this.gameObject.GetComponent<CountryManager>().CountryList[WorldMapCountry.GetComponent<MarkerClickDetector>().CountyID].IsSelected = false;
        }
    }
    
    //Заполнение таблицы с информацией о странах в панели "Список выбранных стран"
    public void FillListWithSelectedCountries()
    {
        EraseCountryRows();
        //print("Количество стран в массиве : " + this.gameObject.GetComponent<CountryManager>().CountryList.Length);

        /*foreach (float value1 in values)
        {
            print(value1);
        }*/

        //Размер массива = Length
        //Пробегаем по массиву стран
        for (int i = 0; i < this.gameObject.GetComponent<CountryManager>().CountryList.Length; i++)
        {
            //Проверяем какие из стран выбраны
            if (this.gameObject.GetComponent<CountryManager>().CountryList[i].IsSelected == true)
            {
                //Добавляем в таблицу строку
                GameObject GO = Instantiate(CountryHorizontalInfoPrefab, GRID.transform);
                SelectedListLength++;

                //Заполняем её информацией из массива стран
                GO.transform.GetChild(0).GetComponent<Text>().text = this.gameObject.GetComponent<CountryManager>().CountryList[i].CountryName;
                GO.transform.GetChild(1).GetComponent<Text>().text = this.gameObject.GetComponent<CountryManager>().CountryList[i].TerritorySize.ToString();// + " км2";
                GO.transform.GetChild(2).GetComponent<Text>().text = this.gameObject.GetComponent<CountryManager>().CountryList[i].PopulationSize.ToString();
                GO.transform.GetChild(3).GetComponent<Text>().text = this.gameObject.GetComponent<CountryManager>().CountryList[i].GDPSize.ToString();// + " трлн.долл.";
            }
        }
    }

    //Очищение таблицы с информацией о странах в панели "Список выбранных стран"
    public void EraseCountryRows()
    {
        GameObject[] CountryInfoRows = GameObject.FindGameObjectsWithTag("CountryHorizontalInfo");
        foreach (GameObject CountryInfoRow in CountryInfoRows)
        {
            SelectedListLength = 0;
            GameObject.Destroy(CountryInfoRow);
        }
    }

}
