import * as signalR from '@microsoft/signalr';
import { getCookie } from '../Utils/cookie';

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost';

export const createSignalRConnection = () => {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${BASE_URL}/hubs/notifications`, {
      accessTokenFactory: () => getCookie('accessToken') || '',
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(signalR.LogLevel.Debug)
    .build();

  // Подписка после подключения
  connection.onreconnected(() => {
    console.log('SignalR reconnected');
    connection.invoke('Subscribe').catch(console.error);
  });

  connection.onclose((error) => {
    console.log('SignalR closed:', error);
  });

  return connection;
};