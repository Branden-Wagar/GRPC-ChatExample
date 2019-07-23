using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerClient
{
    enum StatusCodes
    {
        OK = 200,
        CREATED = 201,
        BADREQUEST = 400,
        UNAUTHORIZED = 401,
        INTERNALSERVER = 500,
        NOTIMPLEMENTED = 501
    }
}
