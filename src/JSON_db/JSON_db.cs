﻿
using System.Text.Json;
using System.Text.Json.Nodes;

namespace aiRobots
{
    public class JSON_db
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public T GetValue<T>(string path, string db_path = "./db.json")
        {
            string json = File.ReadAllText(db_path);
            JsonNode rootNode = JsonNode.Parse(json);

            return this.GetValue<T>(path, rootNode);
        }

        public (T, string) GetValueComment<T>(string path, string db_path = "./db.json")
        {
            string json = File.ReadAllText(db_path);
            JsonNode rootNode = JsonNode.Parse(json);

            T r_value = this.GetValue<T>($"{path}.value", rootNode);

            string r_comment = "";
            try
            {
                r_comment = this.GetValue<string>($"{path}.#", rootNode);
            }
            catch (Exception)
            {

            }

            return (r_value, r_comment);
        }

        public T GetValue<T>(string path, JsonNode db_node)
        {
            string[] pathSegments = path.Split('.');

            JsonNode currentNode = db_node;
            foreach (var segment in pathSegments)
            {
                // 確認當前節點是 JsonObject 並且包含指定的屬性
                if (currentNode is JsonObject obj && obj.ContainsKey(segment))
                {
                    // 移動到下一個節點
                    currentNode = obj[segment];
                }
                else
                {
                    throw new InvalidOperationException($"Path not found: {path}");
                }
            }

            // 將當前節點的值反序列化為指定的類型 T
            return JsonSerializer.Deserialize<T>(currentNode.ToJsonString());
        }        

        private void Remove(string path, ref JsonNode db_node)
        {
            JsonNode currentNode = db_node;

            // 遍歷 JSON 結構，直到目標節點的父節點
            string[] pathSegments = path.Split('.');
            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                string segment = pathSegments[i];

                // 確認節點存在且為 JsonObject
                if (currentNode is JsonObject obj && obj.ContainsKey(segment) && obj[segment] is JsonObject)
                {
                    // 移動到下一個節點
                    currentNode = obj[segment];
                }
                else
                {
                    // 如果某個節點不存在，不須移除，直接返回
                    return;
                }
            }

            var last_segment = pathSegments[^1];
            if (currentNode is JsonObject last_obj && last_obj.ContainsKey(last_segment))
            {
                last_obj.Remove(last_segment);
            }
            return;
        }
        private void Remove(string path, string db_path = "./db.json")
        {
            JsonNode rootNode = new JsonObject();
            try
            {
                string json = File.ReadAllText(db_path);
                rootNode = JsonNode.Parse(json);
            }
            catch (Exception)
            {
            }

            this.Remove(path, ref rootNode);

            // 將修改後的 JSON 寫回到文件中
            string modifiedJson = rootNode.ToJsonString(options);
            File.WriteAllText(db_path, modifiedJson);
        }
        public void SetValue<T>(string path, T value, string comment = "", string db_file = "./db.json")
        {
            if (comment != "")
            {
                this.SetValue($"{path}.#", comment, db_file);
            }
            else
            {
                this.Remove($"{path}.#");
            }

            this.SetValue($"{path}.value", value, db_file);
        }

        public void SetValue<T>(string path, T value, ref JsonNode db_node)
        {
            JsonNode currentNode = db_node;
            string[] pathSegments = path.Split('.');

            // 遍歷路徑，直到倒數第二個元素
            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                string segment = pathSegments[i];

                if (currentNode is JsonObject obj && obj.ContainsKey(segment) && obj[segment].GetValueKind() == JsonValueKind.Object)
                {
                    currentNode = obj[segment];
                }
                else
                {
                    // 如果中間某個節點不存在，則建立新的節點
                    JsonObject newNode = new JsonObject();
                    currentNode[segment] = newNode;
                    currentNode = newNode;
                }
            }

            // 將倒數第一個節點設置為新值
            var last_segment = pathSegments[^1];
            currentNode[last_segment] = JsonNode.Parse(JsonSerializer.Serialize(value));
        }

        public void SetValue<T>(string path, T value, string db_path = "./db.json")
        {
            JsonNode rootNode = new JsonObject();
            try
            {
                string json = File.ReadAllText(db_path);
                rootNode = JsonNode.Parse(json);
            }
            catch (Exception)
            {
            }

            this.SetValue(path, value, ref rootNode);                

            // 將修改後的 JSON 寫入配置文件
            string modifiedJson = rootNode.ToJsonString(options);
            File.WriteAllText(db_path, modifiedJson);
        }
    }
}
