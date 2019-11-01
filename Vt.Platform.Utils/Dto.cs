namespace Vt.Platform.Utils
{
    public static class Dto
    {
        public static TDto Create<TDto>(string createdBy) where TDto : BaseDto, new()
        {
            return new TDto { CreatedBy = createdBy };
        }

        public static TDto Create<TDto>(IBaseService service) where TDto : BaseDto, new()
        {
            return new TDto
            {
                CreatedBy = service.ServiceName,
                ModifiedBy =  service.ServiceName
            };
        }
    }
}