using System;

namespace Gasanov.Exceptions
{
    public class SingletonException<T>:Exception
    {
        public SingletonException(T instance):base($"Экземпляр {typeof(T)} уже существует!")
        {
            
        }
    }
}