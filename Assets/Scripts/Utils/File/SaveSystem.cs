using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using OdinSerializer;

namespace Gasanov.SpeedUtils.FileManagement
 {
    public static class SaveSystem
    {
        /// <summary>
        /// Расширение файла для сохраняемых данных
        /// </summary>
        private const string SaveExtension = ".txt";

        /// <summary>
        /// Папка сохраняемых данных. Оканчивается на "/"
        /// </summary>
        private static readonly string SaveFolder = Application.dataPath + "/GameData/";

        /// <summary>
        /// Инициализирована ли сейчас система сохранений
        /// </summary>
        private static bool IsInitialized;


        private static void Initialize()
        {
            if (IsInitialized == false)
            {
                IsInitialized = true;

                // Проверка существования папки с сохранениями
                if (Directory.Exists(SaveFolder))
                {
                    // Создание отсутствующей папки с сохранениями
                    Directory.CreateDirectory(SaveFolder);
                }
            }
        }

        /// <summary>
        /// Сохранение данных 
        /// </summary>
        /// <param name="fileName">Название сохраняемого файла без расширения</param>
        /// <param name="saveString">Сохраняемые данные</param>
        /// <param name="overwrite">Нужно ли перезаписать файл?</param>
        /// <param name="additionalPath">Дополнительный путь каталогов "xxx/xxx/"</param>
        public static void SaveDataText(string fileName, string saveString, bool overwrite, string additionalPath = "")
        {
            Initialize();

            var saveFileName = fileName;

            // Если нельзя переписать файл
            if (overwrite == false)
            {
                // Уникальный номер файла
                int saveNumber = 0;
                while (File.Exists(SaveFolder + additionalPath + saveFileName + SaveExtension))
                {
                    // Увеличиваем уникальный номер, если такой файл уже существует
                    saveNumber++;
                    // Приписываем номер к названию
                    saveFileName = fileName + "_" + saveNumber;
                }
            }

            // Запись в файл
            File.WriteAllText(SaveFolder + additionalPath + saveFileName + SaveExtension, saveString);
        }
        
        /// <summary>
        /// Сохранение данных 
        /// </summary>
        /// <param name="fileName">Название сохраняемого файла без расширения</param>
        /// <param name="saveBytes">Сохраняемые данные</param>
        /// <param name="overwrite">Нужно ли перезаписать файл?</param>
        /// <param name="additionalPath">Дополнительный путь каталогов "xxx/xxx/"</param>
        public static void SaveDataBytes(string fileName, byte[] saveBytes, bool overwrite, string additionalPath = "")
        {
            Initialize();

            var saveFileName = fileName;

            // Если нельзя переписать файл
            if (overwrite == false)
            {
                // Уникальный номер файла
                int saveNumber = 0;
                while (File.Exists(SaveFolder + additionalPath + saveFileName + SaveExtension))
                {
                    // Увеличиваем уникальный номер, если такой файл уже существует
                    saveNumber++;
                    // Приписываем номер к названию
                    saveFileName = fileName + "_" + saveNumber;
                }
            }

            if (!Directory.Exists(SaveFolder + additionalPath))
                Directory.CreateDirectory(SaveFolder + additionalPath);
            // Запись в файл
            File.WriteAllBytes(SaveFolder + additionalPath + saveFileName + SaveExtension, saveBytes);
        }

        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <param name="additionalPath">Дополнительный путь каталогов "xxx/xxx/"</param>
        public static string LoadDataText(string fileName, string additionalPath = "")
        {
            Initialize();

            // Если файл существует
            if (File.Exists(SaveFolder + additionalPath + fileName + SaveExtension))
            {
                // Данные из файла
                string saveString = File.ReadAllText(SaveFolder + additionalPath + fileName + SaveExtension);
                return saveString;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <param name="additionalPath">Дополнительный путь каталогов "xxx/xxx/"</param>
        public static byte[] LoadDataBytes(string fileName, string additionalPath = "")
        {
            Initialize();

            // Если файл существует
            if (File.Exists(SaveFolder + additionalPath + fileName + SaveExtension))
            {
                // Данные из файла
                byte[] saveBytes = File.ReadAllBytes(SaveFolder + additionalPath + fileName + SaveExtension);
                return saveBytes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Загрузка данных из последнего измененного файла
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <param name="additionalPath">Дополнительный путь каталогов "xxx/xxx/"</param>
        public static string LoadMostRecentData(string fileName, string additionalPath = "")
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(SaveFolder + additionalPath);

            FileInfo[] files = directoryInfo.GetFiles("*" + SaveExtension);

            // Файл с самым поздним временем изменения 
            FileInfo mostRecentFile = null;

            // Проходимся по всем файлам
            foreach (var fileInfo in files)
            {
                if (mostRecentFile == null)
                {
                    mostRecentFile = fileInfo;
                }
                else
                {
                    // Если последняя запись в fileInfo была сделана позже
                    if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                    {
                        mostRecentFile = fileInfo;
                    }
                }
            }

            // Если файл был найден
            if (mostRecentFile != null)
            {
                string saveString = File.ReadAllText(mostRecentFile.FullName);
                return saveString;
            }
            else
            {
                return null;
            }
        }


        public static string ConvertObject<TObject>(TObject serializableObject)
        {
            var json = SerializationUtility.SerializeValue<TObject>(serializableObject, DataFormat.JSON);

            return Encoding.UTF8.GetString(json);
        }

        /// <summary>
        /// Сохраняет объект указанного типа в файл
        /// </summary>
        /// <param name="saveObject"></param>
        /// <param name="fileName"></param>
        /// <param name="overwrite"></param>
        /// <param name="additionalPath"></param>
        public static void SaveObject<TObject>(TObject saveObject, string fileName, bool overwrite, 
            string additionalPath = "")
        {
            var odinJson = SerializationUtility.SerializeValue<TObject>(saveObject, DataFormat.JSON);
            
            SaveDataBytes(fileName,odinJson,overwrite,additionalPath);
        }

        /// <summary>
        /// Загружает объект указанного типа из файла
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="additionalPath"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject LoadObject<TObject>(string fileName, string additionalPath = "")
        {
            var odinJsonBytes = LoadDataBytes(fileName, additionalPath);

            var loadedObject = SerializationUtility.DeserializeValue<TObject>(odinJsonBytes,DataFormat.JSON);

            if (loadedObject == null) return default(TObject);
            
            return loadedObject;
        }

    }

    [System.Serializable]
    public class TestData
    {
        public TestData()
        {

        }

        public TestData(int count, string name, int Count)
        {
            this.count = count;
            this.name = name;
            this.Count = Count;
        }

        private int count;

        public string name;

        public int Count { get; private set; }
    }
}




