namespace OnlineShop.Application.Mapping
{
    using AutoMapper;
    
    public interface IHaveCustomMappings
    {
        void CreateMappings(IProfileExpression profile);
    }
}
