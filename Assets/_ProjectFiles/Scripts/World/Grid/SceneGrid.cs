using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Gasanov.Exceptions;
using Grids;
using TMPro;

using Gasanov.Extensions;
using Gasanov.SpeedUtils;
using Gasanov.SpeedUtils.FileManagement;
using Gasanov.SpeedUtils.MeshUtilities;
using Pathfinding;

namespace World
{
    public class SceneGrid : MonoBehaviour
    {
        public static SceneGrid Instance;
        
        [SerializeField] private Material gridCellMaterial;
        // Настройки сетки
        [SerializeField] private SceneGridSettings gridSettings;
        [SerializeField] private bool generateCellVisual;
        
        // Сетка визуального представления ячеек
        private Grid<GameObject> cellGrid;
        
        // Базовая сетка родительских объектов
        private Grid<Transform> baseGrid;
        
        // Сетка навигации
        private Grid<int> navigationGrid;
        // Сетка для поиска пути
        private LinkedJastar linkedJastar;
        
        

        private MaterialPropertyBlock propertyBlock;

        private Couple<int, int> startSelected;
        private Couple<int, int> goalSelected;

        private void Awake()
        {
            if(Instance != null)
                throw new SingletonException<SceneGrid>(this);

            Instance = this;
            
            SetGridSettings(GridEditor.LoadGridSettings());
            
            // First step
            CheckNavigationGrid();
            InitializeGameObjectHierarchy();
            
            if(generateCellVisual)
                GenerateCellVisual();
            
            // Second step
            CreatePathfinding();
            
            if(propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            
            startSelected = new Couple<int, int>(-1,-1);
            goalSelected = new Couple<int, int>(-1,-1);
        }

        private void Start()
        {

        }

        private void Update()
        {
            var mousePos = UtilsClass.GetMouseWorldPosition();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Если уже было выделение
                if (navigationGrid.InBounds(startSelected.Item1,startSelected.Item2))
                {
                    ChangeCellColor(startSelected.Item1,startSelected.Item2,
                        Color.white);
                    
                    startSelected = new Couple<int, int>(-1,-1);
                }

                if (navigationGrid.InBounds(mousePos))
                {
                    int x, y;
                    navigationGrid.WorldToIndex(mousePos,out x,out y);
                    startSelected = new Couple<int, int>(x,y);
                    
                    if (goalSelected.Item1 == startSelected.Item1 && goalSelected.Item2 == startSelected.Item2)
                    {
                        startSelected = new Couple<int, int>(-1,-1);
                    }
                    else
                    {
                        ChangeCellColor(x,y, Color.blue);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // Если уже было выделение
                if (navigationGrid.InBounds(goalSelected.Item1,goalSelected.Item2))
                {
                    ChangeCellColor(goalSelected.Item1,goalSelected.Item2,
                        Color.white);
                    
                    goalSelected = new Couple<int, int>(-1,-1);
                }
                
                if (navigationGrid.InBounds(mousePos))
                {
                    int x, y;
                    navigationGrid.WorldToIndex(mousePos,out x,out y);
                    goalSelected = new Couple<int, int>(x,y);
                    
                    if (goalSelected.Item1 == startSelected.Item1 && goalSelected.Item2 == startSelected.Item2)
                    {
                        goalSelected = new Couple<int, int>(-1,-1);
                    }
                    else
                    {
                        ChangeCellColor(x,y, Color.cyan);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (startSelected.Item1 == -1 || startSelected.Item2 == -1)
                    return;

                if (goalSelected.Item1 == -1 || goalSelected.Item2 == -1)
                    return;

                var path = linkedJastar.FindPath(
                    linkedJastar.Grid[startSelected.Item1][startSelected.Item2],
                    linkedJastar.Grid[goalSelected.Item1][goalSelected.Item2]);

                for (var i = 0; i < path.Count; i++)
                {
                    ChangeCellColor(path[i].X,path[i].Y,Color.magenta);
                    UtilsClass.CreateWorldText($"{i}", transform,
                        navigationGrid.GetWorldPosition(path[i].X, path[i].Y),5,Color.white);
                }
                
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetHighlight();
            }
        }

        private void OnDrawGizmos()
        {
            
        }

        /// <summary>
        /// Проверка существования всех нужных объектов. Если объекты отсутствуют, то они будут созданы
        /// </summary>
        public void InitializeGameObjectHierarchy()
        {
            var sceneGridGameObject = gameObject;
        
            // Создание объекта сетки сцены
            if (sceneGridGameObject == null)
            {
                sceneGridGameObject = new GameObject("SceneGrid");
            }
        
            baseGrid = new Grid<Transform>(gridSettings.Width,gridSettings.Height,gridSettings.CellSizeHorizontal,
                gridSettings.CellSizeVertical);
            
            
            // События удаления
            baseGrid.OnGridObjectDeleted += (x, y, t) =>
            {
                DestroyImmediate(t.gameObject);
            };
            
            // События появления
            baseGrid.OnGridObjectCreated += (x,y,t) =>
            {
                t = new GameObject("GridCell").transform;
                t.position = baseGrid.GetWorldPosition(x, y);
                t.parent = sceneGridGameObject.transform;
                Debug.Log($"{x}:{y} base");
                baseGrid.SetObject(x,y,t);
                
            };
            
            // Если количество дочерних объектов меньше количества ячеек
            if (sceneGridGameObject.transform.childCount != gridSettings.CellCount)
            {
                // Очищаем все лишние объекты
                if (sceneGridGameObject.transform.childCount != 0)
                {
                    // Проходимся по всем дочерним объектам
                    foreach (Transform child in sceneGridGameObject.transform)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            
                // Создание объектов ячеек
                baseGrid.ForEach((x, y, t) =>
                {
                    t = new GameObject("GridCell").transform;
                    t.position = baseGrid.GetWorldPosition(x, y);
                    t.parent = sceneGridGameObject.transform;
                    baseGrid.SetObject(x,y,t);
                });
            }
            else
            {
                int sceneGridCellIndex = 0;
                // Устанавливаем позицию
                baseGrid.ForEach((x, y, t) =>
                {
                    var cellObject = sceneGridGameObject.transform.GetChild(sceneGridCellIndex++);
                    baseGrid.SetObject(x,y,cellObject);
                    cellObject.position = baseGrid.GetWorldPosition(x, y);
                    
                });   
            }
        }

        /// <summary>
        /// Генерирует визуальное представление ячеек
        /// </summary>
        public void GenerateCellVisual()
        {
            if(propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            
            cellGrid = new Grid<GameObject>(gridSettings.Width,gridSettings.Height,gridSettings.CellSizeHorizontal,
                gridSettings.CellSizeVertical);
            
            cellGrid.OnGridObjectDeleted += (x, y, t) =>
            {
                DestroyImmediate(t.gameObject);
            };
            
            cellGrid.OnGridObjectCreated += (x,y,t) =>
            {
                var quad = MeshUtils.CreateQuadObject(baseGrid.CellSizeHorizontal,
                    baseGrid.CellSizeVertical);
                quad.name = "CellVisual";
                quad.transform.position = new Vector3(x*baseGrid.CellSizeHorizontal,y*baseGrid.CellSizeVertical);
                ApplyMaterialToGameObject(quad, gridCellMaterial);
                quad.transform.parent = baseGrid.GetObject(x,y);
                if (baseGrid.GetObject(x, y) == null)
                    Debug.Log($"null{x}:{y} ");
                cellGrid.SetObject(x,y,quad);
            };
            
            baseGrid.ForEach((x, y, t) =>
            {
                var tCellVisual = t.Find("CellVisual");
                if (t != null && tCellVisual==null)
                {
                    var quad = MeshUtils.CreateQuadObject(baseGrid.CellSizeHorizontal,
                        baseGrid.CellSizeVertical);
                    quad.name = "CellVisual";
                    quad.transform.position = new Vector3(x*baseGrid.CellSizeHorizontal,y*baseGrid.CellSizeVertical);
                    ApplyMaterialToGameObject(quad, gridCellMaterial);
                    quad.transform.parent = t.transform;
                    cellGrid.SetObject(x,y,quad);
                }
                else
                {
                    cellGrid.SetObject(x,y,tCellVisual.gameObject);
                }
            });
        }

        /// <summary>
        /// Подсвечивает навигационную сетку
        /// </summary>
        public void HighlightNavigationGrid()
        {
            if(propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            
            cellGrid.ForEach((x, y, t) =>
            {
                var weight = navigationGrid.GetObject(x, y);

                if (weight != 0)
                {
                    propertyBlock.SetColor("_Color", Color.green);
                }
                else
                {
                    propertyBlock.SetColor("_Color",Color.red);
                }
                
                var renderer = t.GetComponent<Renderer>();
                renderer.SetPropertyBlock(propertyBlock);
            });
        }

        public void ChangeCellColor(int x, int y, Color color)
        {
            if(propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            
            propertyBlock.SetColor("_Color", color);
            
            cellGrid.PopObject(x,y, (t) =>
            {
                var renderer = t.GetComponent<Renderer>();
                renderer.SetPropertyBlock(propertyBlock); 
            });
        }

        // Убирает подсветку
        public void ResetHighlight()
        {
            if(propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            
            propertyBlock.SetColor("_Color", Color.white);
            
            cellGrid.ForEach((x, y, t) =>
            {
                var renderer = t.GetComponent<Renderer>();
                renderer.SetPropertyBlock(propertyBlock);
            });
        }

        /// <summary>
        /// Устанавливает материал на объект
        /// </summary>
        private void ApplyMaterialToGameObject(GameObject gObject,Material material)
        {
            var renderer = gObject.GetComponent<MeshRenderer>();

            if (renderer == null)
            {
                renderer = gObject.AddComponent<MeshRenderer>();
            }

            renderer.material = material;
        }

        /// <summary>
        /// Применяет новые настройки сетки. Изменяет визуальное отображение
        /// </summary>
        public void ApplyGridSettings(SceneGridSettings settings)
        {
            gridSettings = settings;
            
            // Перегенерация
            baseGrid.ApplySceneGridSettings<Transform>(gridSettings);
            cellGrid.ApplySceneGridSettings<GameObject>(gridSettings);
            navigationGrid.ApplySceneGridSettings<int>(gridSettings);
            
            baseGrid.ForEach((x, y, t) =>
            {
                t.transform.position = cellGrid.GetWorldPosition(x, y);
            });
            
            cellGrid.ForEach((x, y, t) =>
            {
                t.transform.position = cellGrid.GetWorldPosition(x, y);
                var mesh = t.GetComponent<MeshFilter>().sharedMesh;
                MeshUtils.ChangeSizeQuadMesh(mesh,cellGrid.CellSizeHorizontal,cellGrid.CellSizeVertical);
                t.GetComponent<MeshFilter>().sharedMesh = mesh;
            });
        }

        /// <summary>
        /// Устанавливает настройки сцены, но не применяет их
        /// </summary>
        /// <param name="settings"></param>
        public void SetGridSettings(SceneGridSettings settings)
        {
            gridSettings = settings;
        }

        /// <summary>
        /// Устанавливает сетку навигации
        /// </summary>
        /// <param name="navGrid"></param>
        public void SetNavigationGrid(Grid<int> navGrid)
        {
            if (navGrid != null)
                navigationGrid = navGrid;
        }

        /// <summary>
        /// Проверка наличия навигационной сетки
        /// </summary>
        private void CheckNavigationGrid()
        {
            if (navigationGrid == null)
                navigationGrid = GridEditor.LoadNavigationGrid();
            
            // Если ее все еще не существует
            if (navigationGrid == null)
            {
                navigationGrid = new Grid<int>(gridSettings.Width,gridSettings.Height,gridSettings.CellSizeHorizontal,
                    gridSettings.CellSizeVertical);
            }
        }

        /// <summary>
        /// Создает сетку поиска пути
        /// </summary>
        private void CreatePathfinding()
        {
            linkedJastar = new LinkedJastar(navigationGrid.Width,navigationGrid.Height);
            navigationGrid.ForEach((x, y, t) =>
            {
                if (t == 0)
                {
                    linkedJastar.Grid[x][y].SetIsWalkableFalse();
                }
                else
                {
                    linkedJastar.Grid[x][y].SetIsWalkableTrue(t);
                }
            });
        }

        public static async Task<Grid<int>> BakeNavigationGrid(LayerMask obstacles, SceneGridSettings gridSettings)
        {
                Debug.Log("Начата генерация навигационной сетки");
                var bakedGrid = new Grid<int>(gridSettings.Width,gridSettings.Height,gridSettings.CellSizeHorizontal,
                    gridSettings.CellSizeVertical);

                await Task.Yield();
                bakedGrid.ForEach((x, y, t) =>
                {
                    var collision = Physics2D.OverlapArea(bakedGrid.GetBottomLeftCornerPosition(x, y),
                        bakedGrid.GetTopRightCornerPosition(x, y), obstacles);
                
                    // Если ячейка свободна
                    if (collision == null)
                    {
                        bakedGrid.SetObject(x, y, 1);
                    }
                });
                
                Debug.Log("Генерация навигационной сетки завершена");

                return bakedGrid;
        }

        /// <summary>
        /// Поиск пути от позиции старта до конечной позиции.
        /// Возвращает null, если путь не был найден
        /// </summary>
        public List<Vector3> FindPath(Vector3 from, Vector3 to)
        {
            if (navigationGrid.InBounds(from) && navigationGrid.InBounds(to))
            {
                int x1, x2, y1, y2;
                
                // from
                navigationGrid.WorldToIndex(from,out x1,out y1);
                // to
                navigationGrid.WorldToIndex(to,out x2,out y2);

                var path = linkedJastar.FindPath(
                    linkedJastar.Grid[x1][y1],
                    linkedJastar.Grid[x2][y2]);

                if (path != null)
                {
                    var vectorizedPath = new List<Vector3>(path.Count);

                    for (var i = 0; i < path.Count; i++)
                    {
                        var point = path[i];
                        vectorizedPath.Add(new Vector3(point.X,point.Y));
                    }

                    return vectorizedPath;
                }

                return null;
            }

            return null;
        }
    }
}