import * as signalR from '@microsoft/signalr';
import { Stock } from '../types/Stock';

export interface IStockService {
    connect(): Promise<void>;
    disconnect(): Promise<void>;
}

export class StockService implements IStockService {

    private connection: signalR.HubConnection;

    private subscribers: Subscriber[] = [];
    private isConnected: boolean = false;
    
    constructor(connection: signalR.HubConnection) {
        this.connection = connection;
    }

    async connect() : Promise<void> {
        if(this.isConnected) 
            return;

        await this.connection.start();
        this.isConnected = true;
    }

    async disconnect() : Promise<void> {
        if(!this.isConnected) 
            return;

        await this.connection.stop();
        this.isConnected = false;
    }
    
    async getStocks(): Promise<Stock[]> {
        return (await this.connection.invoke("GetStocksAsync")).stocks_;
    }

    async subscribe(stockId: number, callback: (value: number) => void): Promise<void> {
        this.subscribers.push(new Subscriber(stockId, callback));
        if(this.subscribers.length == 1) {
            this.connection.on(`subscribeToChanges`, (stock: any) => {
                const subscriber = this.subscribers.find(s => s.stockId === stock.stockId);
                subscriber?.callback(stock.value);
            });
            await this.connection.invoke("SubscribeToChangesAsync");
        }
    }

    //TOODO: Implement unsubscribe
}

class Subscriber {
    stockId: number;
    callback: (value: number) => void;

    constructor(stockId: number, callback: (value: number) => void) {
        this.stockId = stockId;
        this.callback = callback;
    }
}