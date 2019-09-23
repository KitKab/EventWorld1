using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri,
            string authorizationToken = null,
            string authorizationMathod = "Bearer");
        //create stuff
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item,
            string authorizationToken = null,
            string authorizationMathod = "Bearer");
        //edit stuff
        Task<HttpResponseMessage> PutAsync<T>(string uri, T item,
           string authorizationToken = null,
           string authorizationMathod = "Bearer");

        Task<HttpResponseMessage> DeleteAsync(string uri, 
    string authorizationToken = null,
    string authorizationMathod = "Bearer");
    }
}
