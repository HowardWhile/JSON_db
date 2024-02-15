
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

        public void SetValue<T>(string path, T value, string db_path = "./db.json")
        {
            string json = File.ReadAllText(db_path);
            JsonNode rootNode = JsonNode.Parse(json);

            JsonNode currentNode = rootNode;

            string[] pathSegments = path.Split('.');

            // 遍歷路徑，直到倒數第二個元素
            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                string segment = pathSegments[i];

                if (currentNode is JsonObject obj && obj.ContainsKey(segment))
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
            currentNode[pathSegments[^1]] = JsonNode.Parse(JsonSerializer.Serialize(value));

            // 將修改後的 JSON 寫入配置文件
            string modifiedJson = rootNode.ToJsonString(options);
            File.WriteAllText(db_path, modifiedJson);
        }
    }
}
