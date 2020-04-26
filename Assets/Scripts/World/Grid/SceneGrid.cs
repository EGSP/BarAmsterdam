using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Grids;
using TMPro;

using Gasanov.SpeedUtils;

public class SceneGrid : MonoBehaviour
{
    // Количество ячеек по горизонтали
    [SerializeField] private int width;
    // Количество яеек по вертикали
    [SerializeField] private int height;

    // Размер ячейки по горизонтали
    [SerializeField] private float cellHorizontalSize;
    // Размер ячейки по вертикали
    [SerializeField] private float cellVerticalSize;

    private Grid<int> gridInt;
    private TMP_Text[,] textMeshes;

    private void Awake()
    {
        var sceneRoot = new GameObject("SceneGridRoot");
        gridInt = new Grid<int>(width, height, cellHorizontalSize, cellVerticalSize);
        textMeshes = new TMP_Text[width, height];

        gridInt.OnGridObjectChanged += (x, y, t) => textMeshes[x, y].text = t.ToString();

        // Создание текста с координатами
        gridInt.ForEach((x, y, t) =>
        {
            textMeshes[x,y] = UtilsClass.CreateWorldText($"{x}:{y}", sceneRoot.transform, gridInt.GetCenteredWorldPostion(x, y), 5, Color.gray);
        });

        gridInt.PopObject(width / 2, height / 2, (t) => 
        {
            textMeshes[width / 2, height / 2].text = t.ToString();
        });
    }

    private void Start()
    {

    }

    private void Update()
    {
        var mousePos = UtilsClass.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0))
            gridInt.PopObject(mousePos, (x,y,t) => { gridInt.SetObject(x,y,++t); gridInt.GridObjectChanged(x, y, t); });

        if(Input.GetMouseButtonDown(1))
            gridInt.PopObject(mousePos, (x, y, t) => { gridInt.SetObject(x, y, --t); gridInt.GridObjectChanged(x, y, t); });
    }

    private void OnDrawGizmos()
    {
        if(gridInt != null)
        {
            for (int x = 0; x < width; x++)
            {
                // Все вертикальные линии
                Debug.DrawLine(gridInt.GetWorldPosition(x, 0), gridInt.GetWorldPosition(x, height));
            }

            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(gridInt.GetWorldPosition(0, y), gridInt.GetWorldPosition(width, y));
            }

            Debug.DrawLine(gridInt.GetWorldPosition(width, 0), gridInt.GetWorldPosition(width, height));
            Debug.DrawLine(gridInt.GetWorldPosition(0, height), gridInt.GetWorldPosition(width, height));
        }
    }
}

