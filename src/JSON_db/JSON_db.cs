
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
            JsonDocument doc = JsonDocument.Parse(json);

            string[] pathSegments = path.Split('.');

            JsonElement element = doc.RootElement;
            foreach (var segment in pathSegments)
            {
                if (element.TryGetProperty(segment, out JsonElement subElement))
                {
                    element = subElement;
                }
                else
                {
                    throw new InvalidOperationException($"Path not found: {path}");
                }
            }

            return JsonSerializer.Deserialize<T>(element.GetRawText());
        }

        public void SetValue<T>(string path, T value, string comment = "", string db_path = "./db.json")
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
            JsonNode currentNode = rootNode;

            string[] pathSegments = path.Split('.');

            // 遍歷路徑，直到倒數第二個元素
            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                string segment = pathSegments[i];

                if (currentNode is JsonObject obj && obj[segment].GetValueKind() == JsonValueKind.Object)
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

            // 設置節點的註解
            if (comment != "")
            { 
                currentNode[$"#{last_segment}"] = comment;
            }
            else
            {
                if (currentNode is JsonObject obj && obj.ContainsKey($"#{last_segment}"))
                {
                    // 移除 註解
                    obj.Remove($"#{last_segment}");
                }
            }            

            // 將修改後的 JSON 寫入配置文件
            string modifiedJson = rootNode.ToJsonString(options);
            File.WriteAllText(db_path, modifiedJson);
        }
    }
}
