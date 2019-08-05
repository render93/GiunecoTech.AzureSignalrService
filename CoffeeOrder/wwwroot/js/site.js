let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/coffeehub")
        .build();

    // chiamato quando viene effettuata la connessione
    connection.on("ConnectionStarter",
        (sender) => {
            sessionStorage.setItem("senderId", sender);
        }
    );

    // chiamato quando un nuovo client si connette
    connection.on("NewClientConnected",
        () => {
            new Noty({
                theme: 'metroui',
                text: 'Un nuovo utente si è connesso ed è pronto a fare il suo ordine!',
                type: "success",
                timeout: 3000
            }).show();
        }
    );

    // chiamato quando un client si disconnette
    connection.on("OtherClientDisconnected",
        () => {
            new Noty({
                theme: 'metroui',
                text: 'Un utente si è disconnesso.',
                type: "success",
                timeout: 3000
            }).show();
        }
    );

    // chiamato ad ogni aggiornamento dello stato dell'ordine
    connection.on("ReceiveOrderUpdate",
        (update) => {
            const statusDiv = document.getElementById("status");
            statusDiv.innerHTML = update;
        }
    );

    connection.on("NewOrder",
        (order) => {
            new Noty({
                theme: 'metroui',
                text: "Qualcuno ha ordinato il prodotto " + order.productString + " di dimensione " + order.sizeString,
                type: "success",
                timeout: 3000
            }).show();
        }
    );

    // chiamato quando un nuovo ordine inizia ad essere processato
    connection.on("ProceedOrder",
        function () {
            document.getElementById("send").disabled = true;
        });

    // chiamato quando un nuovo ordine è stato completato
    connection.on("OrderComplete",
        function () {
            document.getElementById("send").disabled = false;
        });

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("send").addEventListener("click", e => {
    e.preventDefault();
    const product = document.getElementById("Product").value;
    const size = document.getElementById("Size").value;
    const senderId = sessionStorage.getItem('senderId');

    fetch("/Coffee/OrderCaffee",
        {
            method: "POST",
            body: JSON.stringify({ 'Product': product, 'Size': size, 'SenderId': senderId }),
            headers: {
                'content-type': 'application/json'
            }
        })
        .then(connection.invoke("GetUpdateForOrder"));
});
