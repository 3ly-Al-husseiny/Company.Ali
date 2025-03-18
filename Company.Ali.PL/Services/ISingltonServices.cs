namespace Company.Ali.PL.Services
{
    public interface ISingltonServices
    {
        public Guid Guid { get; set; }
        public string GetGuid();
    }
}
