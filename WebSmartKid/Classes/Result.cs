namespace WebSmartKid.Classes
{
    public class ResObj
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public string guid { get; set; }
    }
    public static class Result
    {
        public static ResObj Return(bool success)
        {
            return new ResObj() { success = success };
        }

        public static ResObj Return(bool success, object data)
        {
            return new ResObj() { success = success, data = data };
        }
        public static ResObj Return(bool success, string? msgCode, object data)
        {
            return new ResObj() { success = success, msg = msgCode, data = data };
        }

        public static ResObj Return(bool success, string? msgCode, object data, string guid)
        {
            return new ResObj() { success = success, msg = msgCode, data = data, guid = guid };
        }
        public static ResObj Return(bool success, string? msgCode)
        {
            return new ResObj() { success = success, msg = msgCode };
        }


    }
}
