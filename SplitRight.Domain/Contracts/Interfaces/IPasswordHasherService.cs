namespace SplitRight.Domain.Contracts.Interfaces
{
    public interface IPasswordHasherService
    {
        string Hash(Object obj, string password);

        bool Verify(Object obj, string password, string passwordHash);
    }
}
