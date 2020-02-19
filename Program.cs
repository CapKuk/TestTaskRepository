using System;
using System.Collections.Generic;
using System.Reflection;

namespace jsonTest
{
    [Serializable]
    public class myJsonClass
    {
        public int a;
        public string s;
        public joppa j;
        public bool b;
    }
    public class joppa
    {
        public int size;
        public joppa()
        {
            size = 100;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            object obj = new myJsonClass();
            ((myJsonClass)obj).a = 0;
            ((myJsonClass)obj).b = false;
            ((myJsonClass)obj).s = "-1";
            ((myJsonClass)obj).j = new joppa();


            FieldInfo[] myFieldInfo = (obj.GetType()).GetFields();

            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                Console.WriteLine("\nName            : {0}", myFieldInfo[i].Name);
                Console.WriteLine("Declaring Type  : {0}", myFieldInfo[i].DeclaringType);
                Console.WriteLine("IsPublic        : {0}", myFieldInfo[i].IsPublic);
                Console.WriteLine("MemberType      : {0}", myFieldInfo[i].MemberType);
                Console.WriteLine("FieldType       : {0}", myFieldInfo[i].FieldType);
                Console.WriteLine("IsFamily        : {0}", myFieldInfo[i].IsFamily);
                Console.WriteLine("Data            : {0}", obj.GetType().GetField(myFieldInfo[i].Name).GetValue(obj));
            }

            Console.WriteLine();
            var flatClass = Flatter(obj);
            foreach(var name in flatClass.Item1)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();
            foreach (var value in flatClass.Item2)
            {
                Console.WriteLine(value);
            }
        }
        static (List<string>, List<object>) Flatter(object obj)
        {
            List<string> FieldsName = new List<string>();
            List<object> FieldsValue = new List<object>();
            FieldInfo[] myFieldInfo = (obj.GetType()).GetFields(); 
            for (int i = 0; i < myFieldInfo.Length; i++)  
            { 
                //узнать, является ли класс базовым. Если нет, то запустить Flatter от этого класса.
                if (isBasic(myFieldInfo[i].FieldType))//не obj, а myFieldInfo[i]
                {
                    FieldsName.Add(myFieldInfo[i].Name);
                    FieldsValue.Add(obj.GetType().GetField(myFieldInfo[i].Name).GetValue(obj));
                }
                else
                {
                    var cortej = Flatter(myFieldInfo[i].GetValue(obj));//от myFieldInfo[i]
                    for(int j = 0; j < cortej.Item1.Count; j++)
                    {
                        FieldsName.Add(cortej.Item1[j]);
                        FieldsValue.Add(cortej.Item2[j]);
                    }
                }
            } 
             
            return (FieldsName, FieldsValue);
        }
        static bool isBasic(Type type)
        {
            //switch (type.ToString())
            //{
            //    case "sbyte":
            //    case "byte":
            //    case "short":
            //    case "ushort":
            //    case "int":
            //    case "uint":
            //    case "long":
            //    case "ulong":

            //    case "float":
            //    case "double":
            //    case "decimal":

            //    case "char":
            //    case "boo;":
            //    case "string":
            //        return true;
            //    default:
            //        return false;
            //}
            if(type.FullName.Substring(0, 6) == "System")
            {
                return true;
            }
            return false;
        }
    }
}
