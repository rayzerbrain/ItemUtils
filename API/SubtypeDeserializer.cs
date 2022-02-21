using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemUtils.API
{
    //T is the base class
    public class SubtypeDeserializer<TBase> where TBase : class
    {
        // Finds highest clas in the hierarchy(T being highest) that retains all properties from the config
        public TBase FindSmallestSubtype(string rawConfig, List<Type> types)
        {
            TBase baseObj = null;

            types.RemoveAll((t) => !t.IsSubclassOf(typeof(TBase)) && !t.Equals(typeof(TBase)));
            
            foreach(Type t in types)
            {
                if (TryDeserialize(rawConfig, t, out TBase newObj))
                {
                    if (baseObj == null || baseObj.GetType().IsSubclassOf(t))
                    {
                        baseObj = newObj;
                        Log.Debug($"Deserialized object type becoming new type {t}", PluginMain.Instance.Config.DebugMode);
                    }
                }
            }
            Log.Debug($"Smallest substype was {baseObj.GetType()}", PluginMain.Instance.Config.DebugMode);
            return baseObj;
        }

        private bool TryDeserialize(string rawConfig, Type subType, out TBase mod)
        {
            mod = (TBase)Loader.Deserializer.Deserialize(rawConfig, subType);
            string allProps = Loader.Serializer.Serialize(mod) + "\n";

            // If any property in the serialized config is not found in the full list of properties, deserialization marked as unsuccessfull
            foreach (string prop in GetRawProperties(rawConfig))
            {
                if (!allProps.Contains(prop))
                {
                    //Log.Debug($"{allProps} was not {subType} because of {prop}", PluginMain.Instance.Config.DebugMode);
                    mod = null;
                    return false;
                }
            }
            return true;
        }
        private List<string> GetRawProperties(string rawConfig)
        {
            string[] lines = rawConfig.Split('\n');
            StringBuilder prop = new StringBuilder();
            List<string> props = new List<string>();

            prop.AppendLine(lines[0].Substring(0, lines[0].Length - 1));
            for (int i = 1; i < lines.Length - 1; i++)
            {
                char c = lines[i][0];
                if (c != ' ' && c != '-')
                {
                    //prop.Remove(prop.Length - 1, 1);
                    props.Add(prop.ToString());
                    prop.Clear();
                }
                prop.AppendLine(lines[i].Substring(0, lines[i].Length-1));
            }
            props.Add(prop.ToString());
            return props;
        }
    }
}
