using System;

namespace SoftwareKobo.JavascriptSerializerDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var testObj = new
            {
                Name = "Tom",
                Age = 18,
                Country = new
                {
                    Zone = "亚洲",
                    Name = "中国"
                },
                Male = true
            };
            JavascriptSerializer serializer = new JavascriptSerializer();
            var json = serializer.Serialize(testObj);
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}