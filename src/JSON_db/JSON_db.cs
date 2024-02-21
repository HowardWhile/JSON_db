
using Json.Path;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

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
                rootNode = JToken.Parse(json);
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
                // 確認 JSON Path 字串是否包含 $ 前綴，如果沒有則添加
                if(json_path.StartsWith("["))
                    json_path = $"${json_path}";
                if (!json_path.StartsWith("$")) 
                    json_path = $"$.{json_path}";

                JToken jpart_parent = "$";

                // 如果找不到指定路徑的 token，則創建對應的結構
                dynamic jpart = db_node;
                var jPath = JsonPath.Parse(json_path);
                
                foreach (var part in jPath.Segments)
                {
                    if (part.IsShorthand) 
                    {
                        // (e.g. `.foo` instead of `['foo']`) 簡寫(.foo)一般都當作object的key
                        var segmnet_name = part.ToString().Trim('.');
                        // 不存在就建立
                        if (jpart[segmnet_name] == null)
                            jpart.Add(new JProperty(segmnet_name, new JObject()));

                        jpart_parent = jpart;
                        jpart = jpart[segmnet_name];

                    }
                    else
                    {
                        // 檢查part是array還是當作object的key
                        if(this.tryParseArrayIndex(part.ToString(), out int array_index))
                        {
                            // 是array的index
                            if(!(jpart_parent.Type == JTokenType.Array))
                            {
                                jpart = new JArray();
                            }
                            else 
                            {

                            }

                            if (jpart[array_index] == null)
                            {
                                var new_array = new JArray();
                                while (new_array.Count < array_index)
                                {
                                    new_array.Add(null);
                                }
                                jpart.Add(new JArray(new_array));
                            }

                            jpart_parent = jpart;
                            jpart = jpart[array_index];
                        }
                        else
                        {
                            // 不是array
                            // 不存在就建立
                            var segmnet_name = part.ToString().Trim('[', ']', '\'');

                            if (jpart[segmnet_name] == null)
                                jpart.Add(new JProperty(segmnet_name, new JObject()));

                            jpart_parent = jpart;
                            jpart = jpart[segmnet_name];
                        }
                    }
                }
                token = jpart;
            }
            
            // 如果找到了 token，則將其值替換為新的值
            token.Replace(JToken.FromObject(value));
        }

        

        private bool tryParseArrayIndex(string part, out int index)
        {
            return int.TryParse(part.Trim('[', ']'), out index);
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
