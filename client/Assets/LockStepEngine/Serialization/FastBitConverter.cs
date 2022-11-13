using System;

namespace LockStepEngine.Serialization
{
    public static class FastBitConverter
    {
        public static int ToInt32(byte[] buffer, int index)
        {
            return (int)ReadData(buffer, index, sizeof(int));
        }
        
        public static uint ToUInt32(byte[] buffer, int index)
        {
            return (uint)ReadData(buffer, index, sizeof(uint));
        }

        public static ushort ToUint16(byte[] buffer, int index)
        {
            return (ushort)ReadData(buffer, index, sizeof(ushort));
        }
        
        private static ulong ReadData(byte[] buffer, int offset, int count)
        {
            ulong result = 0;
            
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < count; i++)
                {
                    result |= (ulong)buffer[offset + i] << i * 8;
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    result |= (ulong)buffer[offset + count - 1 - i] << i * 8;
                }
            }

            return result;
        }
    }
}