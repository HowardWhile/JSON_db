// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;

aiRobots.JSON_db db_tool = new aiRobots.JSON_db();

// set data
db_tool.SetValue("Int_Data", 100);
db_tool.SetValue("Float_Data", 123.321);
db_tool.SetValue("String_Data", "Hello World!!");

// demo get data
Console.WriteLine($"[GetData]: {db_tool.GetValue<int>("Int_Data")}");
Console.WriteLine($"[GetData]: {db_tool.GetValue<float>("Float_Data")}");
Console.WriteLine($"[GetData]: {db_tool.GetValue<string>("String_Data")}");

// set data with json path

//JsonNode json_data = new JsonObject();
//db_tool.SetValue("JSON_Data", "Hello");


// data with comment
//db_tool.SetValue("DataWithComment", 123, comment: "data comment");
//Console.WriteLine($"DataWithComment getValue: {db_tool.GetValueComment<int>("DataWithComment")}");
//Console.WriteLine($"DataWithComment GetValueComment: {db_tool.GetValueComment<int>("DataWithComment")}");

//// modify comment
//db_tool.SetComment("DataWithComment", comment: "new data comment"); 
//Console.WriteLine($"DataWithComment : {db_tool.GetValueComment<int>("DataWithComment")}");

//// clear comment
//db_tool.SetComment("DataWithComment", comment: ""); 
//Console.WriteLine($"DataWithComment : {db_tool.GetValueComment<int>("DataWithComment")}");


//db_tool.SetValue("data.x", 100, comment: "xxx");
//db_tool.SetValue("data.y", 123.321, comment: "yyy");
//db_tool.SetValue("data.z", "Hello", comment: "zzz");


//var test2 = db_tool.GetValueComment<int>("data.x");
//var test3 = db_tool.GetValueComment<double>("data.y");
//var test4 = db_tool.GetValueComment<string>("data.z");
////var test5 = db_tool.getValue<string>("data.w");


//Console.WriteLine($"{test2}");
//Console.WriteLine($"{test3}");
//Console.WriteLine($"{test4}");


