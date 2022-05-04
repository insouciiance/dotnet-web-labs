namespace FinanceManagerServices.Validation
{
    public static class ErrorMessages
    {
        public const string UnexpectedError = "There was an error processing the request.";

        public const string BadCredentials = "The provided credentials were not valid.";

        public const string UserExists = "The user with provided credentials already exists.";

        public const string PasswordsDontMatch = "The passwords don't match.";

        public const string BadTransactionAmount = "The amount of money in the transaction was invalid.";
        
        public const string BadIncomeAmount = "The amount of money in the income was invalid.";
        
        public const string InsufficientBalance = "There is not enough money on the account.";
    }
}
