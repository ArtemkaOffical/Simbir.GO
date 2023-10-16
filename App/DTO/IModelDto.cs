namespace Simbir.GO.App.DTO
{
    public interface IModelDto<T> where T : class
    {
        public T GetModel();
    }
}
