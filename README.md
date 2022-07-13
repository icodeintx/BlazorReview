# Blazor Review

## Feel free to review this code while understanding this is not a complete project but one simple project among a larger solution for real-time Forex trading.

---

This project is for viewing Blazor code.  This project was an internal AmaraCode LLC project needing a web interface for currency trading.  The real project is tied to multiple libraries and microservices which are not included here.  Some of the features of this project include:

1. Azure AD Authentication
2. Custom written Role based Authorization (currently removed from razor components)
3. SignalR communication for real-time commands to/from Blazor website to control and monitor Trader services (other service applications)
4. Dependency Injection
5. REDIS storage for reduction of DB request

---

Other projects which this Blazor application is dependent:

1. Broker service (library) handling all communiations with Forex broker for retrieving real-time currency prices and handling opening and closing of Trades.
2. DataService .Net Core API handling all database interaction along with business rules implemented in a DDD design along with the Repository pattern.
3. Trader services.  A Trader is a worker process which is responsible for handling the trading process and communications betwen the Broker service and the DataService.  The trader service is a generically written service which does not handle the trading strategy but utilized a strategy (library) processing.  Multiple Trader services may be running independently processing for different strategies/traders.
4. PriceStreamer - Pricing is a real-time stream from the broker.  The PriceStreamer handles the stream input and is responsible for transmitting the prices for each Trader in real-time using SignlaR.  Each Trader may be trading different currency pairs at different hours and the PriceStreamer will ensure that each Trader is receiving the correct price data.  All real-time without storing pricing information in the local database.

