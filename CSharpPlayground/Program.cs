// See https://aka.ms/new-console-template for more information
using CSharpPlayground.Enum;

Console.WriteLine("Hello, World!");

var converter = new EnumConverter<FileMode>();
Console.WriteLine(converter.GetValue("hoge"));

