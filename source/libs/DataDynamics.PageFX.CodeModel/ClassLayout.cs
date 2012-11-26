namespace DataDynamics.PageFX.CodeModel
{
    public class ClassLayout
    {
        public ClassLayout(int size, int pack)
        {
            Size = size;
            PackingSize = pack;
        }

	    public int Size { get; private set; }

	    public int PackingSize { get; private set; }
    }
}