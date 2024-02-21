
using Newtonsoft.Json.Linq;

namespace aiRobots
{
    public class JSON_db
    {
        public T GetValue<T>(string path, string db_path = "./db.json")
        {
            string json = File.ReadAllText(db_path);
            JObject jObject = JObject.Parse(json);

            return this.getValue<T>($"{path}.value", jObject);
        }
        public void SetValue<T>(string path, T value, string comment = "", string db_file = "./db.json")
        {
            JToken rootNode = new JObject(); // {}
            //JToken rootNode = new JArray(); // []

            try
            {
                string json = File.ReadAllText(db_file);
                rootNode = JObject.Parse(json);
            }
            catch (Exception)
            {
            }

            this.setValue($"{path}.value", value, rootNode);
            //this.SetComment(json_path, comment, ref rootNode);

            // 將修改後的 JSON 寫入配置文件
            string modifiedJson = rootNode.ToString();
            File.WriteAllText(db_file, modifiedJson);
        }

        //public (T, string) GetValueComment<T>(string json_path, string db_path = "./db.json")
        //{
        //    string json = File.ReadAllText(db_path);
        //    JObject rootNode = JObject.Parse(json);

        //    T r_value = this.getValue<T>($"{json_path}.value", rootNode);

        //    string r_comment = "";
        //    try
        //    {
        //        r_comment = this.getValue<string>($"{json_path}.#", rootNode);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return (r_value, r_comment);
        //}


        //public void SetComment(string json_path, string comment = "", string db_file = "./db.json")
        //{
        //    JsonNode rootNode = new JsonObject();
        //    try
        //    {
        //        string json = File.ReadAllText(db_file);
        //        rootNode = JsonNode.Parse(json);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    this.SetComment(json_path, comment, ref rootNode);

        //    string modifiedJson = rootNode.ToJsonString(options);
        //    File.WriteAllText(db_file, modifiedJson);
        //}

        //public void SetComment(string json_path, string comment, ref JsonNode db_node)
        //{
        //    if (comment != "")
        //    {
        //        this.setValue($"{json_path}.#", comment, ref db_node);
        //    }
        //    else
        //    {
        //        this.remove($"{json_path}.#", ref db_node);
        //    }
        //}



        /* --------------------------------------------------- */
        // private
        /* --------------------------------------------------- */
        private T getValue<T>(string json_path, JToken db_node)
        {
            JToken token = db_node.SelectToken(json_path);
            if (token == null)
            {
                throw new InvalidOperationException($"JsonPath not found: {json_path}");
            }

            // 將 JSON 中的值轉換為指定的類型 T
            return token.ToObject<T>();
        }

        private void setValue<T>(string json_path, T value, JToken db_node)
        {
            JToken token = db_node.SelectToken(json_path);
            if (token == null)
            {
                // 如果找不到指定路徑的 token，則創建對應的結構並設置值
                createTokenAtPath(json_path, value, db_node);
            }
            else
            {
                // 如果找到了 token，則將其值替換為新的值
                token.Replace(JToken.FromObject(value));
            }

        }

        private void createTokenAtPath<T>(string json_path, T value, JToken db_node)
        {
            // 解析路徑
            JToken parentNode = db_node;
            string[] pathSegments = json_path.Split('.');
            foreach (var segment in pathSegments)
            {
                // 確定父節點的類型
                if (parentNode is JObject)
                {
                    JObject obj = (JObject)parentNode;
                    if (!obj.ContainsKey(segment))
                    {
                        // 如果父節點中不包含該段落，則創建一個新的 JsonObject
                        obj[segment] = new JObject();
                    }
                    // 更新父節點為新創建的節點
                    parentNode = obj[segment];
                }
                else if (parentNode is JArray)
                {
                    JArray arr = (JArray)parentNode;
                    // 獲取索引值
                    int index = int.Parse(segment.Trim('[', ']'));
                    // 確認索引值是否在範圍內
                    if (index >= 0 && index < arr.Count)
                    {
                        parentNode = arr[index];
                    }
                    else
                    {
                        throw new InvalidOperationException($"Index out of range: {index}");
                    }
                }
            }

            // 將值設置到最終節點
            parentNode.Replace(JToken.FromObject(value));
        }

        // remove json_path
        //private void remove(string json_path, string db_path)
        //{
        //    JsonNode rootNode = new JsonObject();
        //    try
        //    {
        //        string json = File.ReadAllText(db_path);
        //        rootNode = JsonNode.Parse(json);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    this.remove(json_path, ref rootNode);

        //    // 將修改後的 JSON 寫回到文件中
        //    string modifiedJson = rootNode.ToJsonString(options);
        //    File.WriteAllText(db_path, modifiedJson);
        //}
        //private void remove(string json_path, ref JsonNode db_node)
        //{
        //    JsonNode currentNode = db_node;

        //    // 遍歷 JSON 結構，直到目標節點的父節點
        //    string[] pathSegments = json_path.Split('.');
        //    for (int i = 0; i < pathSegments.Length - 1; i++)
        //    {
        //        string segment = pathSegments[i];

        //        // 確認節點存在且為 JsonObject
        //        if (currentNode is JsonObject obj && obj.ContainsKey(segment) && obj[segment] is JsonObject)
        //        {
        //            // 移動到下一個節點
        //            currentNode = obj[segment];
        //        }
        //        else
        //        {
        //            // 如果某個節點不存在，不須移除，直接返回
        //            return;
        //        }
        //    }

        //    var last_segment = pathSegments[^1];
        //    if (currentNode is JsonObject last_obj && last_obj.ContainsKey(last_segment))
        //    {
        //        last_obj.Remove(last_segment);
        //    }
        //    return;
        //}

    }
}
