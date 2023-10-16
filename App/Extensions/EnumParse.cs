namespace Simbir.GO.App.Extensions
{
    public static class EnumParse
    {
        public static T Parse<T>(this Enum @enum, string value) where T : class
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
