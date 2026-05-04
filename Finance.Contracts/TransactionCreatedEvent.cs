namespace Finance.Contracts
{
    public record TransactionCreatedEvent(int UserId,
                                          string Email,
                                          string AccountName,
                                          decimal Balance,
                                          decimal SpentAmount
    );  
}