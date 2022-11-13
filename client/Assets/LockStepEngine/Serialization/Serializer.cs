using System;

namespace LockStepEngine.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Limited : Attribute
    {
        public bool le255;
        public bool le65535;

        public Limited()
        {
            le255 = false;
            le65535 = true;
        }

        public Limited(bool isLess255)
        {
            le255 = isLess255;
            le65535 = !isLess255;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class SelfImplementAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class UDPAttribute : Attribute
    {
    
    }

    public interface ISerializable
    {
        void Serialize(Serializer write);
        void Deserialize(Deserializer reader);
    }

    public interface ISerializablePacket
    {
        byte[] ToBytes();
        void FromBytes(byte[] bytes);
    }
    
    public class Serializer
    {
        private byte[] data;
        private int position;
        
        public byte[] Data { get { return data; } }
        public int Length { get { return position; } }
        public int Position { get {return position; } }
    }
}