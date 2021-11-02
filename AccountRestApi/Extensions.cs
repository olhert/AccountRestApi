using System;
using System.Text;
using AccountRestApi.Controllers;

namespace AccountRestApi
{
    public class Extensions
    {
        public static string ParseAuthToken(string authHeader)
        {
            var fromBaseToBytes = Convert.FromBase64String(authHeader); 
            var key = "ARAPn1FJlgqe2DIM0lOFxUBj";
            var decodedDataBytes = EncodeDecode.AesEncodeDecode.Decode(fromBaseToBytes, Encoding.ASCII.GetBytes(key));
            var decodedString = Encoding.ASCII.GetString(decodedDataBytes);
             
            return AuthToken.FromJson(decodedString).UserId;
        }
    }
}


