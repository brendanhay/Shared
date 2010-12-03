using System.IO;
using System.Web;

namespace Infrastructure.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static string GetPayload(this HttpRequestBase self)
        {
            self.InputStream.Position = 0;

            using (var reader = new StreamReader(self.InputStream)) {
                return reader.ReadToEnd();
            }
        }
    }
}
