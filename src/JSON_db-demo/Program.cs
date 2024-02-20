// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;

aiRobots.JSON_db db_tool = new aiRobots.JSON_db();

db_tool.SetValue("data", 123, "data_comment");
db_tool.SetValue("data", 123, "xxxx");
db_tool.SetValue("data", 123);
db_tool.SetValue("data", 123, "data_comment");
db_tool.SetValue("data.x", 100);
db_tool.SetValue("data.y", 123.321);
db_tool.SetValue("data.z", "Hello");

var test1 = db_tool.GetValue<JsonNode>("data");
var test2 = db_tool.GetValue<int>("data.x");
var test3 = db_tool.GetValue<double>("data.y");
var test4 = db_tool.GetValue<string>("data.z");
//var test5 = db_tool.GetValue<string>("data.w");

Console.WriteLine($"{test1}");
Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");


