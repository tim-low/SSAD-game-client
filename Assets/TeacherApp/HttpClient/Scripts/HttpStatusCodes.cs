using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network_Http
{
    // 1xx: Informational	Communicates transfer protocol-level information.
    // 2xx: Success Indicates that the client’s request was accepted successfully.
    // 3xx: Redirection Indicates that the client must take some additional action in order to complete their request.
    // 4xx: Client Error   This category of error status codes points the finger at clients.
    // 5xx: Server Error   The server takes responsibility for these error status codes.
    
    public class HttpStatusCodes
    {
        public const int OK = 200;
        public const int BadRequest = 400;
        public const int InternalServerError = 500;
    }
}