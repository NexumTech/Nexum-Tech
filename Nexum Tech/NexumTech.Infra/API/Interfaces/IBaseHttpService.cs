namespace NexumTech.Infra.API.Interfaces
{
    public interface IBaseHttpService
    {
        public Task<T> CallMethod<T>(string url, HttpMethod method, string token, object data = null, Dictionary<string, string> headers = null);
    }
}
