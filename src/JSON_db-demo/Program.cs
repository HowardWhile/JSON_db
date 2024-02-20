// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;

aiRobots.JSON_db db_tool = new aiRobots.JSON_db();

db_tool.SetValue("DataWithComment", 123, comment: "comment for it data");
db_tool.SetValue("DataWithComment", 123, comment: "xxxx"); // test modify comment
db_tool.SetValue("DataWithComment", 123, comment: ""); // clear comment
db_tool.SetValue("data.x", 100, comment: "xxx");
db_tool.SetValue("data.y", 123.321, comment: "yyy");
db_tool.SetValue("data.z", "Hello", comment: "zzz");

var test1 = db_tool.GetValueComment<JsonNode>("data");
var test11 = db_tool.GetValue<JsonNode>("data");
var test2 = db_tool.GetValueComment<int>("data.x");
var test3 = db_tool.GetValueComment<double>("data.y");
var test4 = db_tool.GetValueComment<string>("data.z");
//var test5 = db_tool.GetValue<string>("data.w");

Console.WriteLine($"{test1}");
Console.WriteLine($"{test11}");
Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");


