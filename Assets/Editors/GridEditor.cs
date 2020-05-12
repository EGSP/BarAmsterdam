using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gasanov.SpeedUtils.FileManagement;
using UnityEngine;
using UnityEditor;

using Grids;
using UnityEditor.UIElements;
using World;
using UnityEngine.UIElements;

public partial class GridEditor: EditorWindow
{
    private static readonly string DataPath = "GridEditorData/";

    // Текущие настройки сетки
    private SceneGridSettings gridSettings;

    // Текущий объект сцены
    private SceneGrid sceneGrid;
    
    private VisualElement workspace;

    private bool isNavHighlighted;
    private Task<Grid<int>> navGenerationTask;
    
    
    [MenuItem("GridEditor/Open")]
    private static void ShowWindow()
    {
        var window = GetWindow<GridEditor>();
        
        window.titleContent = new GUIContent("Grid Editor");
        
        window.minSize = new Vector2(250,300);
        window.maxSize = new Vector2(500,600);
    }
    
    /// <summary>
    /// Загрузка настроек сетки
    /// </summary>
    private void InitializeGridEditor()
    {
        var settings = LoadGridSettings();
        // SceneGridSettings test;
        
        if (settings.IsInitialized==false)
        {
            settings = new SceneGridSettings();
            settings.Width = 10;
            settings.Height = 10;
            settings.CellSizeHorizontal = 1f;
            settings.CellSizeVertical = 0.5f;
            settings.IsInitialized = true;

            gridSettings = settings;
            SaveGridSettings();
            return;
            // test = SaveSystem.LoadObject<SceneGridSettings>("gridSettings", path);
        }
        gridSettings = settings;

        var navGrid = LoadNavigationGrid();
        // Загрузка сетки навигации
        if (navGrid == null)
        {
            navGrid = new Grid<int>(gridSettings.Width, gridSettings.Height, gridSettings.CellSizeHorizontal,
                gridSettings.CellSizeVertical);
            
            SaveNavigationGrid(navGrid);
        }
    }

    private void OnEnable()
    {
        InitializeGridEditor();
        CheckSceneGrid();
        
        var root = rootVisualElement;
        
        root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editors/GridEditor_Style.uss"));

        var editorWindowAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/GridEditor_Window.uxml");

        var editorWindowVisual = editorWindowAsset.CloneTree();
        root.Add(editorWindowVisual);

        var toolButtons = editorWindowVisual.Query<Button>(null, "tool-button");
        toolButtons.ForEach(SetupTools);

        workspace = editorWindowVisual.Q<VisualElement>(null, "workspace");
        
    }
    
    /// <summary>
    /// Проверка объекта сетки сцены. Если объект отсутствует, то он будет создан
    /// </summary>
    private void CheckSceneGrid()
    {
        GameObject sceneGridGameObject = null;
        sceneGrid = GameObject.FindObjectOfType<SceneGrid>(); 
        
        if (sceneGrid == null)
        {
            sceneGridGameObject = new GameObject("SceneGrid",typeof(SceneGrid));
            // Получение компонента сетки сцены
            sceneGrid = sceneGridGameObject.GetComponent<SceneGrid>();
        }
        else
        {
            sceneGridGameObject = sceneGrid.gameObject;
        }

        sceneGrid.SetGridSettings(gridSettings);
        sceneGrid.InitializeGameObjectHierarchy();
        sceneGrid.GenerateCellVisual();
        
        // Сетка навигации
        sceneGrid.SetNavigationGrid(LoadNavigationGrid());
        
    }
    
    

    private void SetupTools(Button button)
    {

        var parentName = button.parent.name;

        switch (parentName)
        {
            case "SelectTool":
                break;
                
            case "NavigationTool":
                button.clicked += OnNavigationToolSelected;
                break;
            
            case "Settings":
                button.clicked += OnSettingsToolSelected;
                break;
        }
        button.tooltip = parentName;
    }

    public void OnNavigationToolSelected()
    {
        var navigationWindow = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/NavigationGrid_Form.uxml");
        var navigationWindowVisual = navigationWindow.CloneTree();
        
        workspace.Clear();
        workspace.Add(navigationWindowVisual);

        var showButton = navigationWindowVisual.Q<Button>("show-button");
        var generateButton = navigationWindowVisual.Q<Button>("generate-button");

        var collisionField = navigationWindowVisual.Q<LayerMaskField>("collision-mask");

        showButton.clicked += () =>
        {
            if (isNavHighlighted == true)
            {
                sceneGrid.ResetHighlight();
                isNavHighlighted = false;
            }
            else
            {
                sceneGrid.HighlightNavigationGrid();
                isNavHighlighted = true;
            }
        };

        generateButton.clicked += () =>
        {
            if (navGenerationTask == null || navGenerationTask.IsCompleted)
            {
                sceneGrid.ResetHighlight();
                isNavHighlighted = false;

                navGenerationTask = SceneGrid.BakeNavigationGrid(collisionField.value, gridSettings)
                    .ContinueWith(t =>
                    {
                        sceneGrid.SetNavigationGrid(t.Result);
                        SaveNavigationGrid(t.Result);
                        Debug.Log("Установка навигационной сетки завершена");

                        return t.Result;
                    });
            }
            else
            {
                Debug.Log("Генерация навигационной сетки еще не завершена");
            }
        };
    }

    /// <summary>
    /// Метод обрабатывающий нажатие на кнопку настроек
    /// </summary>
    public void OnSettingsToolSelected()
    {
        var settingsWindow = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/SceneGridSettings_Form.uxml");
        var settingsWindowVisual = settingsWindow.CloneTree();
        
        workspace.Clear();
        workspace.Add(settingsWindowVisual);

        var visualWidth = settingsWindowVisual.Q<IntegerField>("width");
        var visualHeight = settingsWindowVisual.Q<IntegerField>("height");
        var visualCellSizeHorizontal = settingsWindowVisual.Q<FloatField>("cellSizeHorizontal");
        var visualCellSizeVertical = settingsWindowVisual.Q<FloatField>("cellSizeVertical");
        
        // Set gridSettings values
        visualWidth.value = gridSettings.Width;
        visualHeight.value = gridSettings.Height;
        visualCellSizeHorizontal.value = gridSettings.CellSizeHorizontal;
        visualCellSizeVertical.value = gridSettings.CellSizeVertical;
        
        // Accept button setup
        var acceptButton = settingsWindowVisual.Q<Button>("accept-button");
        acceptButton.tooltip = "Применяет новые настройки для сетки";
        acceptButton.clicked += () =>
        {
            int width, height;
            float cellSizeHorizontal, cellSizeVertical;
            // Парсинг переменных
            width = visualWidth.value;
            height = visualHeight.value;

            cellSizeHorizontal = visualCellSizeHorizontal.value;
            cellSizeVertical = visualCellSizeVertical.value;

            gridSettings.Width = width;
            gridSettings.Height = height;
            gridSettings.CellSizeHorizontal = cellSizeHorizontal;
            gridSettings.CellSizeVertical = cellSizeVertical;
            
            SaveGridSettings();
            
            sceneGrid.ApplyGridSettings(gridSettings);
            
            Debug.Log("Настройки сетки были сохранены");
        };
        
        // Default button setup
        var defaultButton = settingsWindowVisual.Q<Button>("default-button");
        defaultButton.tooltip = "Возвращает прежние настройки сетки";
        defaultButton.clicked += () =>
        {
            // Set gridSettings values
            visualWidth.value = gridSettings.Width;
            visualHeight.value = gridSettings.Height;
            visualCellSizeHorizontal.value = gridSettings.CellSizeHorizontal;
            visualCellSizeVertical.value = gridSettings.CellSizeVertical;
        };
    }

    

    /// <summary>
    /// Сохранение текущих настроек сетки
    /// </summary>
    private void SaveGridSettings()
    {
        var path = "SceneGrids/";
        SaveSystem.SaveObject<SceneGridSettings>(gridSettings,"gridSettings",true,DataPath+path);
    }

    private void SaveNavigationGrid(Grid<int> navigationGrid)
    {
        var path = "SceneGrids/";
        SaveSystem.SaveObject<Grid<int>>(navigationGrid,"navigationGrid",true,DataPath+path);
    }

    /// <summary>
    /// Загружает настройки сетки
    /// </summary>
    /// <returns></returns>
    public static SceneGridSettings LoadGridSettings()
    {
        var path = "SceneGrids/";
        return  SaveSystem.LoadObject<SceneGridSettings>("gridSettings", DataPath+path);
    }

    /// <summary>
    /// Загружает сетку навигации
    /// </summary>
    public static Grid<int> LoadNavigationGrid()
    {
        var path = "SceneGrids/";
        return SaveSystem.LoadObject<Grid<int>>("navigationGrid", DataPath + path);
    }

    
}

