using System.Runtime.InteropServices.WindowsRuntime;
using LockStepEngine.Serialization;

namespace LockStepEngine.Serialization
{

    public static class NetMsgExtension
    {
        public static T Parse<T>(this Deserializer reader) where T : ISerializable, new()
        {
            var val = new T();
            val.Deserialize(reader);
            return val;
        }
    }
    
    public static class ArrayExtension
    {
        public static bool EqualsEx(this byte[] lhs, byte[] rhs)
        {
            if ((lhs == null) != (rhs == null))
            {
                return false;
            }

            if (lhs == null)
            {
                return true;
            }

            var count = lhs.Length;
            if (count != rhs.Length)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                if (lhs[i] != rhs[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool EqualsEx<T>(this T[] lhs, T[] rhs) where T : class
        {
            if ((lhs == null) != (rhs == null))
            {
                return false;
            }

            if (lhs == null)
            {
                return true;
            }
            
            var count = lhs.Length;
            if (count != rhs.Length)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                var a = lhs[i];
                var b = rhs[i];
                
                if ((a == null) != (b == null))
                {
                    return false;
                }

                if (a == null)
                {
                    continue;
                }

                if (!a.Equals(b))
                {
                    return false;
                }
            }

            return true;
        }
    }
}