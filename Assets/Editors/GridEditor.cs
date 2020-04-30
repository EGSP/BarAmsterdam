using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gasanov.SpeedUtils.FileManagement;
using UnityEngine;
using UnityEditor;

using Grids;
using OdinSerializer;
using UnityEngine.UIElements;

public class GridEditor: EditorWindow
{
    private readonly string DataPath = "GridEditoData/";

    // Текущие настройки сетки
    private SceneGridSettings gridSettings;

    // Текущий объект сцены
    private SceneGrid sceneGrid;

    
    // Сетка всех ячеек
    private Grid<Transform> baseGrid;
    
    // Текущая сетка навигации
    private Grid<bool> navigationGrid;

    
    [MenuItem("GridEditor/Open")]
    private static void ShowWindow()
    {
        var window = GetWindow<GridEditor>();
        
        window.titleContent = new GUIContent("Grid Editor");
        
        window.minSize = new Vector2(250,300);
        window.maxSize = new Vector2(500,600);
        
        
    }

    private void OnEnable()
    {
        var root = rootVisualElement;
        
        root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editors/GridEditor_Style.uss"));

        var editorWindow = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/GridEditor_Window.uxml");

        var editorContainer = editorWindow.CloneTree();
        root.Add(editorContainer);

        var toolButtons = root.Query<Button>(null, "tool-button");
        toolButtons.ForEach(SetupTools);
        
        LoadGridSettings();
        CheckGameObjectHierarchy();
    }

    private void SetupTools(Button button)
    {
        Debug.Log(button.name);

        var parentName = button.parent.name;

        switch (parentName)
        {
            case "NavigationTool":
                break;
        }
        button.tooltip = parentName;
    }

    private void LoadNavigationGrid()
    {
        
    }

    /// <summary>
    /// Загрузка настроек сетки
    /// </summary>
    private void LoadGridSettings()
    {
        var path = "ScenenGrids/";
        
        var settings = SaveSystem.LoadObject<SceneGridSettings>("gridSize", path);
        SceneGridSettings test;
        
        if (settings == null|| settings.IsInitialized==false)
        {
            settings = new SceneGridSettings();
            settings.Width = 10;
            settings.Height = 10;
            settings.CellSizeHorizontal = 1f;
            settings.CellSizeVertical = 0.5f;
            settings.IsInitialized = true;
            
            SaveSystem.SaveObject<SceneGridSettings>(settings,"gridSettings",true, path);
            test = SaveSystem.LoadObject<SceneGridSettings>("gridSettings", path);
        }

        gridSettings = settings;
    }

    private void CheckGameObjectHierarchy()
    {
        var sceneGridGameObject = GameObject.Find("SceneGrid");
        
        // Создание объекта сетик сцены
        if (sceneGridGameObject == null)
        {
            sceneGridGameObject = new GameObject("SceneGrid",typeof(SceneGrid));
        }

        // Получение компонента сетки сцены
        sceneGrid = sceneGridGameObject.GetComponent<SceneGrid>();
        
        baseGrid = new Grid<Transform>(gridSettings.Width,gridSettings.Height,gridSettings.CellSizeHorizontal,
            gridSettings.CellSizeVertical);
        
        // Если количество дочерних объектов меньше количества ячеек
        if (sceneGridGameObject.transform.childCount < gridSettings.CellCount)
        {
            // Очищаем все лишние объекты
            if (sceneGridGameObject.transform.childCount != 0)
            {
                // Проходимся по всем дочерним объектам
                foreach (Transform child in sceneGridGameObject.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
            // Создание объектов ячеек
            baseGrid.ForEach((x, y, t) =>
            {
                t = new GameObject("Grid_Test").transform;
                t.position = baseGrid.GetWorldPosition(x, y);
                t.parent = sceneGridGameObject.transform;
            });
        }
        else
        {
            // Устанавливаем позицию
            baseGrid.ForEach((x, y, t) =>
            {
                var cellObject = sceneGridGameObject.transform.GetChild(x * baseGrid.Width + y);
                t = cellObject;
                t.position = baseGrid.GetWorldPosition(x, y);
            });   
        }
    }
    
    

    [System.Serializable]
    private class SceneGridSettings
    {
        [OdinSerialize]
        public bool IsInitialized { get; set; }
        [OdinSerialize]
        public int Width { get; set; }
        [OdinSerialize]
        public int Height { get; set; }

        public int CellCount => Width * Height;

        [OdinSerialize]
        public float CellSizeHorizontal { get; set; }
        
        [OdinSerialize]
        public float CellSizeVertical { get; set; }
    }
}

