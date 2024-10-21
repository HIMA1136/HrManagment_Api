namespace Application.Model.Common;
public class ResponseResult
{
    public bool IsSuccssed { get; set; }
    public string Message { get; set; }
    public object Obj { get; set; }
    public string Token { get; set; }
}

public class FireBasResult
{
    public bool IsSuccssed { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string fireBaseToken { get; set; }
}