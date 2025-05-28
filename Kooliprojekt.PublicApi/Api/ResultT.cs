namespace KooliProjekt.PublicApi.Api
{
    public class Result<T> : Result
    {
        public T Value { get; set; }
    }
}