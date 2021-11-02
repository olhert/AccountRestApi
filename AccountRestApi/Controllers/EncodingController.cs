using System;
using System.Text;
using AccountRestApi.DB;
using Microsoft.AspNetCore.Mvc;

namespace AccountRestApi.Controllers
{
    public class EncodingController: ControllerBase
    {
        [HttpPost("/encoding")]
        public IActionResult EncodeUser(AuthToken authToken)
        {
            var user = new AuthToken { UserId = authToken.UserId, TokenExpiredDate = DateTime.UtcNow.AddHours(1) };
            var key = "ARAPn1FJlgqe2DIM0lOFxUBj";
            var userAsJson = user.GetJson();
            
            var encodedBytes = EncodeDecode.AesEncodeDecode.Encode(Encoding.ASCII.GetBytes(userAsJson), Encoding.ASCII.GetBytes(key));
            var baseString = Convert.ToBase64String(encodedBytes);

            return Ok(new StatusModel {Status = baseString});
        }

        [HttpPost("/decoding")]
        public IActionResult DecodeUser(string baseString, string key)
        {
            var fromBaseToBytes = Convert.FromBase64String(baseString);
            var decodedDataBytes = EncodeDecode.AesEncodeDecode.Decode(fromBaseToBytes, Encoding.ASCII.GetBytes(key));
            var decodedString = Encoding.ASCII.GetString(decodedDataBytes);

            var decodeUser = AuthToken.FromJson(decodedString);

            return Ok(new StatusModel {Status = $"{decodeUser}"});
        }
    }
}