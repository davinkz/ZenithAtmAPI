namespace ZenithBankATM.API.Database.DataContext
{
    public class Scripts
    {
    }
}
Create Procedure sp_MakeDeposit (
	@AccountId	UniqueIdentifier,
	@Amount		Decimal(18, 2),
	@Reference	Varchar(30),
	@ValueDate	DateTime
)
As

Begin

	-- validations
	If (Select Count(Id) From Accounts Where Id = @AccountId) = 0
	Begin
		Raiserror('Account information not found', 16, 1)
		Return
	End
	If (Select Count(Id) From Transactions Where Reference = @Reference) > 0
	Begin
		Raiserror('There is already a transaction captured with this reference number', 16, 1)
		Return
	End
	If @Amount <= 0
	Begin
		Raiserror('Deposit amount must be greater than 0', 16, 1)
		Return
	End
	If @ValueDate > GetDate()
	Begin
		Raiserror('Value date cannot be greater than today.', 16, 1)
		Return
	End

	-- create transaction entry
	Begin Transaction

	Insert Into Transactions 
	(	Id, AccountId, TransactionType, Reference, DateCaptured, ValueDate, Amount, Narration)
	Values 
	(NewId(), @AccountId,				1,@Reference,	GetDate(),@ValueDate,@Amount,'Deposit')
	If @@ERROR != 0
		Goto _cancel

	-- update account balance
	Update Accounts
	Set Balance = IsNull(Balance, 0) + @Amount
	Where Id = @AccountId
	If @@ERROR != 0
		Goto _cancel

	_save:
		Commit Transaction
		Goto _end

	_cancel:
		Rollback Transaction

	_end:
End
Go

Create Procedure sp_MakeWithdrawal (
	@AccountId	UniqueIdentifier,
	@Amount		Decimal(18, 2),
	@Reference	Varchar(30),
	@ValueDate	DateTime
)
As

Begin

	-- validations
	If (Select Count(Id) From Accounts Where Id = @AccountId) = 0
	Begin
		Raiserror('Account information not found', 16, 1)
		Return
	End
	If (Select Count(Id) From Transactions Where Reference = @Reference) > 0
	Begin
		Raiserror('There is already a transaction captured with this reference number', 16, 1)
		Return
	End
	If @Amount <= 0
	Begin
		Raiserror('Deposit amount must be greater than 0', 16, 1)
		Return
	End
	If @ValueDate > GetDate()
	Begin
		Raiserror('Value date cannot be greater than today.', 16, 1)
		Return
	End

	Declare @AccountBalance Decimal(18, 2)
	Select @AccountBalance = Sum(Amount) From Transactions Where AccountId = @AccountId
	If @Amount > IsNull(@AccountBalance, 0)
	Begin
		Raiserror('Insufficient account balance.', 16, 1)
		Return
	End

	-- create transaction entry
	Begin Transaction

	Insert Into Transactions 
	(	Id, AccountId, TransactionType, Reference, DateCaptured, ValueDate, Amount, Narration)
	Values 
	(NewId(), @AccountId,				1,@Reference,	GetDate(),@ValueDate,-@Amount,'Withdrawal')
	If @@ERROR != 0
		Goto _cancel

	-- update account balance
	Update Accounts
	Set Balance = IsNull(Balance, 0) - @Amount
	Where Id = @AccountId
	If @@ERROR != 0
		Goto _cancel

	_save:
		Commit Transaction
		Goto _end

	_cancel:
		Rollback Transaction

	_end:
End
Go