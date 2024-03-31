using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCFodmapProject.Shared;

public class Response
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }

    public Response()
    {
    }

    public Response(bool succeeded, string message)
    {
        Errors = new Dictionary<string, string[]>();
        Succeeded = succeeded;
        Message = message;
    }

    public Response(bool succeeded, string message, Dictionary<string, string[]> errors)
    {
        Succeeded = succeeded;
        Message = message;
        Errors = errors;
    }

    public Response(string message)
    {
        Errors = new Dictionary<string, string[]>();
        Succeeded = false;
        Message = message;
    }
}
