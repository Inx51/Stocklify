# Protocol Gateway

---

Generates fake stock values and changes for driving realtime stock market fake applications.

# SignalR Hubs
## /hub/stockValueServiceHub

### GetSocksAsync() : getStocks(): Stocks
Returns a list of all stocks and their current values.

### SubscribeToChangesAsync() : subscribeToChanges(): streamOf\<Stock>
A "stream" of changes to the stock prices.


---

A stream of changes to the stock prices can be subscribed to over gRPC streaming.

# HTTP
## GET /stocks
Returns a list of all stocks and their current values.

---

Get all stocks and their current values at the time of calling.
