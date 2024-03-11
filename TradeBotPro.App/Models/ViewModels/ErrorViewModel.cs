public class ErrorViewModel
{
    public List<string> ErrorMessages { get; set; } = new List<string>();
    public string RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public ErrorViewModel AddError(string errorMessage)
    {
        ErrorMessages.Add(errorMessage);
        return this;
    }
}