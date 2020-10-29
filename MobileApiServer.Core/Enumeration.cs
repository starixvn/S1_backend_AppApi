namespace MobileApiServer.Core
{
    public enum WalletStatus
    {
        Normal = 1,
        Um = 2,
        Error = 3,
    }
    public enum Direction
    {
        Add = 1,
        Deduct = 2
    }
    public enum CustStatus
    {
        Running = 1,
        Suspend = 2,
        Closed = 3,
    }

    /// <summary>
    /// For login
    /// </summary>
    public enum ErrorStatus
    {
        NotAvailable = -3,
        WrongPassword = -2,
        NotExists = -1,
        Success = 0
    }

    public enum InnerResultError
    {
        UnknownError = -1,
        NullOrEmpty = -2,
        OperationFailed =-3
    }

    /// <summary>
    /// Error message for API result
    /// </summary>
    public enum PublicError
    {
        Success = 0,
        InternalException = -1,
        NullOrEmpty = -2,
        OperationFailed = -3,
        UserOrPasswordNotMatch = -4,
        NotAvailable = -5,
        TargetNotFound = -6,
        InvalidTarget = -7,
        ExternalException = -8,
        SystemMaintenance = -98,
        UnknownError = -99
    }
}