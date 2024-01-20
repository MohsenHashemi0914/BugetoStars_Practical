
// Create a connection to the hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/onlineVisitorHub") // Specify the URL of the hub
    .build();

// Start the connection
connection.start();