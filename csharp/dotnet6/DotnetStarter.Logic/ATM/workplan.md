**atm kata**

Outside in API
 - void Withdraw(int amount)

outside in tests
 - asserts console o/p


data
 - quantity of bills and coin that exists in the ATM

behavior
 - construct ATM with given Dictionary<Money, int> => ATM
 - withdraw from ATM -> room for extension => ATM
 - Dictionary<Money, int> to string => Display
future iterations
 - handle not enough money case
 - handle not enough change case


 **Design**

ATM
 - ATM(MoneyCollection)
 - MoneyCollection Withdraw(amount)

MoneyCollection
 - Dictionary<Money, int> map
 - MoneyCollection(Dictionary<Money, int>)
 - Value

Money (value object)
 - Value
 - Type

Display
 - print(MoneyCollection) => writes to console
