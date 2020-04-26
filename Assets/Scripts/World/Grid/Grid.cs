using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

namespace Grids
{
    public class Grid<TObject>
    {
        /// <summary>
        /// Количество ячеек по горизонтали
        /// </summary>
        public int width { get; private set; }

        /// <summary>
        ///  Количество ячеек по вертикали
        /// </summary>
        public int height { get; private set; }

        /// <summary>
        ///  Размер ячейки по горизонтали
        /// </summary>
        public float cellHorizontalSize { get; private set; }

        /// <summary>
        /// Размер ячейки по вертикали
        /// </summary>
        public float cellVerticalSize { get; private set; }

        /// <summary>
        /// Массив объектов сетки
        /// </summary>
        public TObject[,] gridArray { get; private set; }

        /// <summary>
        /// Вызывается при изменении значения сетки. Передает два индекса и изменяемы объект
        /// </summary>
        public event Action<int,int,TObject> OnGridObjectChanged = delegate { };

        public Grid(int width, int height, float cellHorizontalSize, float cellVerticalSize)
        {
            this.width = width;
            this.height = height;

            this.cellHorizontalSize = cellHorizontalSize;
            this.cellVerticalSize = cellVerticalSize;

            gridArray = new TObject[width, height];
        }


        /// <param name="createTObject">Функция создания объекта</param>
        public Grid(int width, int height, float cellHorizontalSize, float cellVerticalSize, Func<TObject> createTObject)
            : this(width, height, cellHorizontalSize, cellVerticalSize)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridArray[x, y] = createTObject();
                }
            }
        }

        /// <param name="createTObject">Функция создания объекта. Получает координаты ячейки</param>
        public Grid(int width, int height, float cellHorizontalSize, float cellVerticalSize, Func<int, int, TObject> createTObject)
            : this(width, height, cellHorizontalSize, cellVerticalSize)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridArray[x, y] = createTObject(x, y);
                }
            }
        }




        /// <summary>
        /// Получение позиции в мировых координатах
        /// </summary>
        public Vector3 GetWorldPosition(int x,int y)
        {
            return new Vector3(x * cellHorizontalSize, y * cellVerticalSize);
        }

        /// <summary>
        /// Получение позиции центра ячейки в мировых координатах
        /// </summary>
        public Vector3 GetCenteredWorldPostion(int x,int y)
        {
            return new Vector3(x * cellHorizontalSize, y * cellVerticalSize) + new Vector3(cellHorizontalSize, cellVerticalSize) * 0.5f;
        }

        /// <summary>
        /// Получение индексов из мировой позиции. 
        /// Возвращает отрицательные числа тоже
        /// </summary>
        public void WorldToIndex(Vector3 worldPos, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPos.x / cellHorizontalSize);
            y = Mathf.FloorToInt(worldPos.y / cellVerticalSize);
        }

        //
        // Я сделал анонимные методы для того,
        // чтобы при отсутствии объекта или некорректных координатах не возвращать throw или null
        //  
        // Рекомендуется использовать PopObject вместо прямого получения объекта с помощью GetObject
        //

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Выполняемый метод</param>
        public void PopObject(int x, int y, Action<TObject> popAction)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                popAction(gridArray[x, y]);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Выполняемый метод</param>
        public void PopObject(Vector3 worldPos, Action<TObject> popAction)
        {
            int x, y;
            WorldToIndex(worldPos,out x,out y);
            // Проверка на отрицательный индекс внутри
            PopObject(x, y, popAction);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Передает кроме объекта индексы в сетке</param>
        public void PopObject(int x,int y, Action<int,int,TObject> popAction)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                popAction(x, y, gridArray[x, y]);
        }

        /// <summary>
        /// Вызывает метод popAction, передавая объект полученный по индексам. 
        /// Если объект не может быть получен, то popAction не вызовется
        /// </summary>
        /// <param name="popAction">Передает кроме объекта индексы в сетке</param>
        public void PopObject(Vector3 worldPos, Action<int,int,TObject> popAction)
        {
            int x, y;
            WorldToIndex(worldPos, out x, out y);
            // Проверка на отрицательный индекс внутри
            PopObject(x, y, popAction);
        }

        /// <summary>
        /// Получение объекта по индексу
        /// </summary>
        public TObject GetObject(int x,int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return gridArray[x, y];

            return default(TObject);
        }

        /// <summary>
        /// Получение объекта по мировым координатам
        /// </summary>
        public TObject GetObject(Vector3 worldPos)
        {
            int x, y;
            WorldToIndex(worldPos, out x, out y);
            return GetObject(x, y);
        }

        /// <summary>
        /// Устанавливает новый объект (ссылочный тип).
        /// Изменяет значение (значимый тип)
        /// </summary>
        public void SetObject(int x,int y, TObject newObject)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = newObject;
                OnGridObjectChanged(x, y, newObject);
            }
        }

        /// <summary>
        /// Устанавливает новый объект (ссылочный тип).
        /// Изменяет значение (значимый тип)
        /// </summary>
        public void SetObject(Vector3 worldPos, TObject newObject)
        {
            int x, y;
            WorldToIndex(worldPos,out x, out y);
            SetObject(x, y, newObject);
        }

        /// <summary>
        /// Проходится по каждой ячейке и вызывает метод
        /// </summary>
        /// <param name="action">Вызываемый метод. Аргументами являются индексы в сетке</param>
        public void ForEach(Action<int,int,TObject> action)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    action(x, y,gridArray[x,y]);
                }
            }
        }

        /// <summary>
        /// Вызывать при изменении объекта (ссылочный тип)
        /// </summary>
        /// <param name="value">Изменяемый объект</param>
        public void GridObjectChanged(int x,int y,TObject value)
        {
            OnGridObjectChanged(x,y,value);
        }
    }
}