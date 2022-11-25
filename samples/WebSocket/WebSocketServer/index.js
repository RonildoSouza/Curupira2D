/**
 * https://github.com/websockets/ws
 * npm install ws
 */
import WebSocket, { WebSocketServer } from 'ws';

const wss = new WebSocketServer({ port: 6001 });

wss.on('connection', (ws, req) => {
    const ip = req.socket.remoteAddress;
    console.log("connection:", wss.clients.size, ip);

    ws.on('message', (data, isBinary) => {
        wss.clients.forEach((client) => {
            if (client !== ws && client.readyState === WebSocket.OPEN) {
                client.send(data, { binary: isBinary });
            }
        });
    });

    ws.on("close", () => {
        console.log("close:", wss.clients.size);
    });
});