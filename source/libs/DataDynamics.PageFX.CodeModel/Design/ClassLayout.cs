namespace DataDynamics.PageFX.CodeModel
{
    public class ClassLayout
    {
        public ClassLayout(int size, int pack)
        {
            _size = size;
            _pack = pack;
        }

        public int Size
        {
            get { return _size; }
        }
        private readonly int _size; 

        public int PackingSize
        {
            get { return _pack; }
        }
        private readonly int _pack;
    }
}