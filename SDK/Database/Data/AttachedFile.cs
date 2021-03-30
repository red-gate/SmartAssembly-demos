namespace SmartAssembly.DatabaseSample.Data
{
    public class AttachedFile
    {
        public string Key { get; set;}
        public string FileName { get; set; }
        public byte[] Bytes;

        public override string ToString()
        {
            return string.Format("{0}={1}, bytes={2}", Key, FileName, Bytes.Length);
        }
    }
}
