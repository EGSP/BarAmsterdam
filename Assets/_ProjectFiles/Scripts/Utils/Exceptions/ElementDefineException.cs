using System;

namespace Gasanov.Exceptions
{
    public class ElementDefineException:Exception
    {
        public ElementDefineException() : base("Элемент уже был определен")
        {
            
        }
    }
}