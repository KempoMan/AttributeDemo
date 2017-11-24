using System;
using System.Collections.Generic;
using System.Linq;

namespace ReflectionTesting
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var people = new List<Person>();

            var x = new Person
            {
                Name = "Alan", Age = 44, FavoriteColor = "red", DoNotInclude = "testing", FavoriteWord = "Developer"
            };
            var y = new Person {Name = "Sully", Age = 11, DoNotInclude = "testing"};
            var z = new Person {Name = "Seamus", Age = 8, FavoriteColor = ""};
            var w = new Person {Name = "test", FavoriteColor = "red"};
            
            people.Add(x);
            people.Add(y);
            people.Add(z);
            people.Add(w);

            foreach (var person in people)
            {
                foreach (var property in person.GetType().GetProperties())
                {
                    var isMySpecialAttribute = property
                        .GetCustomAttributes(typeof(CustomRequiredAttribute), true)
                        .Any();

                    var isObsceleteAttribute = property
                        .GetCustomAttributes(typeof(ObsoleteAttribute), true)
                        .Any();
                    
                    if (isObsceleteAttribute)
                    {
                        Console.WriteLine("OBSOLETE Property:" + property.Name + " | Value:" + property.GetValue(person));
                        continue;
                    }

                    if (!isMySpecialAttribute) continue;
                    
                    if (string.IsNullOrEmpty(property.GetValue(person)?.ToString()))
                    {
                        Console.WriteLine("Property: " + property.Name + " | Value: [BLANK!]");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Property:" + property.Name + " | Value:" + property.GetValue(person));
                    }
                }

                Console.WriteLine("============================================");
            }
            Console.WriteLine("press a key.");
            Console.ReadKey();
        }
    }

    public class Person
    {
        [CustomRequired]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Name { get; set; }

        [CustomRequired]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int Age { get; set; }

        [CustomRequired]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string FavoriteColor { get; set; }
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string DoNotInclude { get; set; }

        [CustomRequired]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string FavoriteWord { get; set; }
        
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CustomRequiredAttribute : Attribute
    {
        
    }
}