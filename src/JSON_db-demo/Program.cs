﻿// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;

aiRobots.JSON_db db_tool = new aiRobots.JSON_db();

db_tool.SetValue("data.x", 100);
db_tool.SetValue("data.y", 123.321);
db_tool.SetValue("data.z", "Hello");
db_tool.SetValue("value", 123);

var test1 = db_tool.GetValue<JsonNode>("data");
var test2 = db_tool.GetValue<int>("data.x");
var test3 = db_tool.GetValue<double>("data.y");
var test4 = db_tool.GetValue<string>("data.z");

Console.WriteLine($"{test1}");
Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");


