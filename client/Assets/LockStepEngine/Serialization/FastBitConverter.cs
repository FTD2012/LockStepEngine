using System;
using System.Runtime.InteropServices;

namespace LockStepEngine.Serialization
{
    public static class FastBitConverter
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct ConverterHelperDouble
        {
            [FieldOffset(0)] public ulong Along;
            [FieldOffset(1)] public double Adouble;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ConverterHelperFloat
        {
            [FieldOffset(0)] public int Aint;
            [FieldOffset(1)] public float Afloat;
        }

        public static void GetBytes(byte[] buffer, int startIndex, double value)
        {
            ConverterHelperDouble ch = new ConverterHelperDouble { Adouble = value };
            WriteData(buffer, startIndex, ch.Along);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, float value)
        {
            ConverterHelperFloat ch = new ConverterHelperFloat { Afloat = value };
            WriteData(buffer, startIndex, ch.Aint);
        }

        public static void GetBytes(byte[] buffer, int startIndex, bool value)
        {
            buffer[startIndex] = (byte) (value ? 1 : 0);
        }

        public static void GetBytes(byte[] buffer, int startIndex, short value)
        {
            WriteData(buffer, startIndex, value);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, ushort value)
        {
            WriteData(buffer, startIndex, (short)value);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, int value)
        {
            WriteData(buffer, startIndex, value);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, uint value)
        {
            WriteData(buffer, startIndex, (int)value);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, long value)
        {
            WriteData(buffer, startIndex, (ulong)value);
        }
        
        public static void GetBytes(byte[] buffer, int startIndex, ulong value)
        {
            WriteData(buffer, startIndex, value);
        }
        
        public static short ToInt16(byte[] buffer, int index)
        {
            return (short)ReadData(buffer, index, sizeof(short));
        }
        
        public static ushort ToUInt16(byte[] buffer, int index)
        {
            return (ushort)ReadData(buffer, index, sizeof(ushort));
        }
        
        public static int ToInt32(byte[] buffer, int index)
        {
            return (int)ReadData(buffer, index, sizeof(int));
        }
        
        public static uint ToUInt32(byte[] buffer, int index)
        {
            return (uint)ReadData(buffer, index, sizeof(uint));
        }
        
        public static long ToInt64(byte[] buffer, int index)
        {
            return (long)ReadData(buffer, index, sizeof(long));
        }
        
        public static ulong ToUInt64(byte[] buffer, int index)
        {
            return ReadData(buffer, index, sizeof(ulong));
        }

        public static float ToSingle(byte[] buffer, int index)
        {
            int i = (int)ReadData(buffer, index, sizeof(float));
            ConverterHelperFloat ch = new ConverterHelperFloat { Aint = i };
            return ch.Afloat;
        }

        public static double ToDouble(byte[] buffer, int index)
        {
            ulong i = ReadData(buffer, index, sizeof(double));
            ConverterHelperDouble ch = new ConverterHelperDouble { Along = i };
            return ch.Adouble;
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

        private static void WriteData(byte[] buffer, int offset, short data)
        {
#if BIGENDIAN
            buffer[offset + 1] = (byte)(data);
            buffer[offset    ] = (byte)(data >> 8);
#else
            buffer[offset    ] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
#endif
        }
        
        private static void WriteData(byte[] buffer, int offset, int data)
        {
#if BIGENDIAN
            buffer[offset + 3] = (byte)(data);
            buffer[offset + 2] = (byte)(data >> 8);
            buffer[offset + 1] = (byte)(data >> 16);
            buffer[offset    ] = (byte)(data >> 24);
#else
            buffer[offset    ] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
#endif
        }
        
        private static void WriteData(byte[] buffer, int offset, ulong data)
        {
#if BIGENDIAN
            buffer[offset + 7] = (byte)(data);
            buffer[offset + 6] = (byte)(data >> 8);
            buffer[offset + 5] = (byte)(data >> 16);
            buffer[offset + 4] = (byte)(data >> 24);
            buffer[offset + 3] = (byte)(data >> 32);
            buffer[offset + 2] = (byte)(data >> 40);
            buffer[offset + 1] = (byte)(data >> 48);
            buffer[offset    ] = (byte)(data >> 56);
#else
            buffer[offset    ] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
            buffer[offset + 4] = (byte)(data >> 32);
            buffer[offset + 5] = (byte)(data >> 40);
            buffer[offset + 6] = (byte)(data >> 48);
            buffer[offset + 7] = (byte)(data >> 56);
#endif
        }
    }
}