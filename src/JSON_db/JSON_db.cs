
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace aiRobots
{
    public class JSON_db
    {
        // 創建新的路徑並設置值
        private JToken createNewParameterPath(JObject jsonObject, string parameter_path)
        {
            // 分割路徑
            string[] pathParts = parameter_path.Split('.');

            // 用於建立路徑的遞迴方法
            JToken CreatePath(JToken node, int index)
            {
                if (index >= pathParts.Length)
                {
                    return node;
                }

                // 確保物件存在
                if (node is JObject obj)
                {
                    string pathPart = pathParts[index];

                    // 如果當前層次不存在這個屬性，則創建它
                    if (obj.TryGetValue(pathPart, out JToken child))
                    {
                        // 子屬性已存在，遞迴進入下一層
                        return CreatePath(child, index + 1);
                    }
                    else
                    {
                        // 子屬性不存在，創建新的子屬性並遞迴進入下一層
                        JToken newChild = new JObject();
                        obj.Add(pathPart, newChild);
                        return CreatePath(newChild, index + 1);
                    }
                }
                else
                {
                    // 如果當前節點不是物件，無法創建子屬性，則拋出異常
                    throw new InvalidOperationException($"Invalid JSON path at {string.Join(".", pathParts, 0, index)}");
                }
            }

            // 開始遞迴創建路徑
            CreatePath(jsonObject, 0);

            return jsonObject.SelectToken(parameter_path);
        }

        public void Save(int value, string database_path = @"./db.json", string parameter_path = @"data")
        {
            string jsonContent = "{}";
            if (File.Exists(database_path))
            {
                // Read the json file content to the string
                jsonContent = File.ReadAllText(database_path);
            }

            // 將JSON字串轉換成JObject物件
            JObject jsonObject = JObject.Parse(jsonContent);
            //var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var token = jsonObject.SelectToken(parameter_path);
            if (token == null)
            {
                token = this.createNewParameterPath(jsonObject, parameter_path);
            }

            // 將JToken設定到指定路徑
            token.Replace(value);

            string modifiedJsonContent = jsonObject.ToString(formatting: Formatting.Indented);

            // 將修改後的內容寫回JSON檔案中
            File.WriteAllText(database_path, modifiedJsonContent);
        }
    }
}
