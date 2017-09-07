using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Json;

namespace Framework.File
{
    public class FileToJsonSerializationHelper<T> : FileSerializationHelper<T>
    {
        public override void Serialize(System.IO.Stream stream, T data)
        {
            string json = JsonConverter.SerializeObject(data);
            using (System.IO.TextWriter writer = new System.IO.StreamWriter(stream))
            {
                writer.Write(json);
            }
        }

        public override T Deserialize(System.IO.Stream stream)
        {
            T data;
            using (System.IO.TextReader reader = new System.IO.StreamReader(stream))
            {
                data = JsonConverter.DeserializeObject<T>(reader.ReadToEnd());
            }
            return data;
        }
    }
}
