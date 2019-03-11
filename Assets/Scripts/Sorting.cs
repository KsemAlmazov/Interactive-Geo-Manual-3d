using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorting : MonoBehaviour {
    
    public Image SortDirection1;
    public Image SortDirection2;
    public Image SortDirection3;

    public Transform GRID;

    private float[] OrderedList;
    private bool IsFloat = false;

    private int TempInt;
    private float TempFloat;

    //Сортировка по территории
    public void SortByTerritory()
    {        
        if (SortDirection1.rectTransform.localScale == new Vector3(1.0f, 1.0f, 1.0f))
        {
            SortLoop(1);

            //Вызвали Quicksort алгоритм. Сортировка идёт по возрастанию
            System.Array.Sort(OrderedList);

            //Повернули иконку сортировки вниз (по убыванию)
            SortDirection1.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        }
        else
        {
            SortLoop(1);
            
            System.Array.Sort(OrderedList);
            //отразили массив (По убыванию)
            System.Array.Reverse(OrderedList);

            //Повернули иконку сортировки вниз (по возрастанию)
            SortDirection1.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        RearrangeList(1);
    }

    //Сортировка по населению
    public void SortByPopulation()
    {
        if (SortDirection2.rectTransform.localScale == new Vector3(1.0f, 1.0f, 1.0f))
        {
            SortLoop(2);
            
            System.Array.Sort(OrderedList);

            SortDirection2.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        }
        else
        {
            SortLoop(2);

            System.Array.Sort(OrderedList);
            System.Array.Reverse(OrderedList);

            SortDirection2.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        RearrangeList(2);
    }

    //Сортировка по ВВП
    public void SortByGDP()
    {
        if (SortDirection3.rectTransform.localScale == new Vector3(1.0f, 1.0f, 1.0f))
        {
            SortLoop(3);

            System.Array.Sort(OrderedList);

            SortDirection3.rectTransform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        }
        else
        {
            SortLoop(3);

            System.Array.Sort(OrderedList);
            System.Array.Reverse(OrderedList);

            SortDirection3.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        RearrangeList(3);
    }

    //H это номер GetChild параметра. 
    //1 - текст с числом территории
    //2 - текст с числом населением
    //3 - текст с числом ВВП
    private void SortLoop(int H)
    {
        if (H == 3) IsFloat = true;

        for (int i = 0; i < this.gameObject.GetComponent<MarkerTouchManager>().SelectedListLength; i++)
        {
            //Считали размер территории объекта

            if (IsFloat == false)
            {
                TempInt = int.Parse(GRID.transform.GetChild(i).GetChild(H).GetComponent<Text>().text);
                //Расширили временный массив на 1 слот
                System.Array.Resize(ref OrderedList, i + 1);

                //Записали число в временный массив
                OrderedList[i] = TempInt;
            }
            else
            {
                TempFloat = float.Parse(GRID.transform.GetChild(i).GetChild(H).GetComponent<Text>().text);

                System.Array.Resize(ref OrderedList, i + 1);
                
                OrderedList[i] = TempFloat;
            }

        }

        IsFloat = false;
    }

    //X это номер GetChild параметра. 
    //1 - текст с числом территории
    //2 - текст с числом населением
    //3 - текст с числом ВВП
    private void RearrangeList(int X)
    {
        for (int u = 0; u < OrderedList.Length; u++)
        { Debug.Log(OrderedList[u]); }

        //Если считаем ВВП, то используем Float вместо Int
        if (X == 3) IsFloat = true;

        for (int j = 0; j < OrderedList.Length; j++)
        {
            for(int k = 0; k < OrderedList.Length; k++)
            {
                //Записали число территории
                if (IsFloat == false)
                {
                    TempInt = int.Parse(GRID.transform.GetChild(k).GetChild(X).GetComponent<Text>().text);
                }
                else
                {
                    TempFloat = float.Parse(GRID.transform.GetChild(k).GetChild(X).GetComponent<Text>().text);
                }

                //Сравнили число из упорядоченного списка с числом территории
                if (OrderedList[j] == TempInt && IsFloat == false)
                {
                    Debug.Log(OrderedList[j]+ " " + TempInt);
                    //Установили эту строчку на первое место в списке
                    GRID.transform.GetChild(k).transform.SetSiblingIndex(j);
                }
                else if (OrderedList[j] == TempFloat && IsFloat == true)
                {
                    GRID.transform.GetChild(k).transform.SetSiblingIndex(j);
                }

            }
        }

        IsFloat = false;
    }
}
