// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;

aiRobots.JSON_db db_tool = new aiRobots.JSON_db();

db_tool.SetValue("DataWithComment", 123, comment: "comment for it data");
db_tool.SetComment("DataWithComment", comment: "xxxx"); // test modify comment
db_tool.SetComment("DataWithComment", comment: ""); // clear comment

Console.WriteLine($"DataWithComment Demo1-1: {db_tool.GetValueComment<JsonNode>("data")}");
Console.WriteLine($"DataWithComment Demo1-2: {db_tool.GetValue<JsonNode>("data")}");


db_tool.SetValue("data.x", 100, comment: "xxx");
db_tool.SetValue("data.y", 123.321, comment: "yyy");
db_tool.SetValue("data.z", "Hello", comment: "zzz");


var test2 = db_tool.GetValueComment<int>("data.x");
var test3 = db_tool.GetValueComment<double>("data.y");
var test4 = db_tool.GetValueComment<string>("data.z");
//var test5 = db_tool.GetValue<string>("data.w");


Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");


