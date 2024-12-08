namespace OnlineShop.Application.Services.Interfaces
{
    public interface IBaseService
    {
        bool IsGuidValid(string? id, ref Guid parseGuid);
    }
}
