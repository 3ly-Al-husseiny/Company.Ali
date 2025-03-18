namespace Company.Ali.PL.Services
{
    public class SingltonServices : ISingltonServices
    {
        public SingltonServices()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }
        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
