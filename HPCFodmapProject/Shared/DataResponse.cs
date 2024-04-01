using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared;

public class DataResponse<T> : Response
{
    public T Data { get; set; }
    public DataResponse()
    { }
    public DataResponse(T data)
    {
        Succeeded = true;
        Data = data;
    }

    public static implicit operator DataResponse<T>(List<UserEditDto> v)
    {
        throw new NotImplementedException();
    }
}
