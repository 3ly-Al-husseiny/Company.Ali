namespace Company.Ali.PL.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }
        public string GetGuid();
    }
}
