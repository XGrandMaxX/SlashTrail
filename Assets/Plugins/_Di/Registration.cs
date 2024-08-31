using System;

namespace _Di
{
    public class Registration
    {
        public Func<object> Factory { get; set; }
        public object Instance { get; set; }
        public bool IsSingleton { get; set; }
    }
}