namespace LockStepEngine.Serialization
{
    public class Deserializer
    {
        private byte[] data;
        private int dataSize;
        private int position;
        private int offset;

        public byte[] RawData { get { return data; } }
        public int RawDataSize { get { return dataSize; } }
        public int UserDataOffset { get { return offset; } }
        public int UserDataSize { get { return dataSize - offset; } }
        public int Position { get { return position; } }
        public int AvailableBytes{ get {return dataSize - position; } }
        public bool IsNull { get { return data == null; } }
        public bool IsEnd { get { return position == dataSize; } }

        
        public Deserializer()
        {
            
        }

        public Deserializer(byte[] data)
        {
            SetData(data);
        }

        public Deserializer(byte[] data, int offset)
        {
            SetData(data, offset);
        }

        public Deserializer(byte[] data, int offset, int dataSize)
        {
            SetData(data, offset, dataSize);
        }
        
        public void SetData(Serializer dataWriter)
        {
            data = dataWriter.Data;
            dataSize = dataWriter.Length;
            position = 0;
            offset = 0;
        }

        public void SetData(byte[] _data)
        {
            data = _data;
            dataSize = _data.Length;
            position = 0;
            offset = 0;
        }

        public void SetData(byte[] _data, int _offset)
        {
            data = _data;
            dataSize = _data.Length;
            position = _offset;
            offset = _offset;
        }
        
        public void SetData(byte[] _data, int _offset, int _dataSize)
        {
            data = _data;
            dataSize = _dataSize;
            position = _offset;
            offset = _offset;
        }

        public void SetPosition(int pos)
        {
            position = pos;
        }

        public bool SkipLength(int length)
        {
            var destLength = position + length;
            if (destLength > dataSize)
            {
                GLog.Error($"Skip len is out of range dataSize:{dataSize} position:{position}  skipLength:{length}");
                return false;
            }

            position += length;
            return true;
        }


    }
}