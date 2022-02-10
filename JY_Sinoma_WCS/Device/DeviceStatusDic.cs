using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JY_Sinoma_WCS.Device
{
    public class DeviceStatusDic:Dictionary<string,string>
    {
        public String getDesc(string type,string code)
        {
            string key = type + "," + code;
            if (base.ContainsKey(key))
                if (base[key] == "0")
                    return "0";
                else
                    return base[key] + "_" + code;
            else
                return code;
        }
    }
}
