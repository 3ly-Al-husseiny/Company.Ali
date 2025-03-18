namespace Company.Ali.PL.Services
{
    public class TransientService : ITransientService
    {
        public TransientService()
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
