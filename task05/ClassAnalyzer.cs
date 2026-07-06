using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq; 

namespace _ClassAnalyzer
{
    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods() => _type.GetMethods().Select(met => met.Name);

        public IEnumerable<string> GetMethodParams(string methodname)
        {
            MethodInfo method = _type.GetMethod(methodname);
            Type returnType = method.ReturnType;
            return method.GetParameters().Select(param => $"параметры {param.ParameterType.Name} и {param.Name}").Append($"тип {returnType.Name}");
        }

        public IEnumerable<string> GetAllFields() => _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(fild => fild.Name);

        public IEnumerable<string> GetProperties() => _type.GetProperties().Select(prop => prop.Name);

        public bool HasAttribute<T>() where T : Attribute => _type.IsDefined(typeof(T), inherit: true);
    }
}
