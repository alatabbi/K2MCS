# K2MCS (K2 memory cache server)

The aim of this project is to construct a very basic single node key-value memory cache server.
The solution contains the following: 

-K2 Server: a memory  cache server that maintain a dictionary of key/value pairs, K2 allows N key-value pairs to be added to the dictionary at any given time, as further entries are added, the oldest entries should be evicted first, the oldest entries should be evicted first, the number N is configurable and can be supplied by the user when starting the server as a command line argument, for example "K2Server.exe 1000" would start an instance of the K2 Server with memory size of 1000 items.
K2-Server interacts with its clients via a TCP connection using the supplied port and ip-address. 

-K2 Client App: a simple console application that interacts with K2-Server via a TCP connection, once the client is connected to a K2-server, the user can start sending requests to the server as follows: 

Operations

PUT 
To put a key-value pair:

PUT\n

key\n

value\n

The server returns: OK\n - when put succeeds or FAIL\n if the put fails.

GET
To get a value  of a given key

GET\n

key\n

The server returns value\n ("value" would be an empty string if there is no entry for that key)

Note: Both key and value are ASCII strings and cannot contain a new-line (\n) character.

- Also the solution contains a simple unit testing project to test various functionality of K2-Server. 
Finally, K2-Server contains a simple client API to access the server, by adding a reference to the project, the client api can be used as follows:

K2Client k2c = new K2Client("127.0.0.1", "3000");
var r = k2c.PutInCache("someKey", "someValue");
var v = k2c.GetFromCache("someKey");


- To implement the fixed-size memory feature and auxiliary queue object is used to evict the oldest item from the memory  as further entries are added, in the case of the maximum limit is reached.
- The TCP connection for K2-Server and client is implemented using TcpListener and TcpClient resp.

