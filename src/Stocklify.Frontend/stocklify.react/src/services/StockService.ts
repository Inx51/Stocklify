import * as signalR from '@microsoft/signalr';
import { Stock } from '../types/Stock';

export interface IStockService {
    connect(): Promise<void>;
    disconnect(): Promise<void>;
    startSubscribeToChanges(): Promise<void>;
    stopSubscribeToChanges(): Promise<void>;
}

export class StockService implements IStockService {

    stocks : any = {};

    private connection: signalR.HubConnection;

    private subscribers: number = 0;
    private isConnected: boolean = false;

    private listeners: Array<() => void> = [];
    
    constructor(connection: signalR.HubConnection) {
        this.connection = connection;
    }

    async connect() : Promise<void> {
        if(this.isConnected) 
            return;

        await this.connection.start();
        await this.loadStocks();
        this.isConnected = true;
    }

    async disconnect() : Promise<void> {
        if(!this.isConnected) 
            return;

        await this.connection.stop();
        this.isConnected = false;
    }
    
    private async loadStocks(): Promise<any> {
        let result = (await this.connection.invoke("GetStocksAsync")).stocks_;
        for(let i = 0; i < result.length; i++) {
            this.stocks[`${i}`] = result[i];
        }
    }

    async startSubscribeToChanges(): Promise<void> {
        if(this.subscribers == 0) {
            await this.connection.invoke("SubscribeToChangesAsync");
            this.connection.on("subscribeToChanges", (stock: any) => {
                this.stocks[`${stock.stockId}`].value = stock.value;
                this.listeners.forEach(listener => listener());
            });
        }
        this.subscribers++;
    }

    async stopSubscribeToChanges(): Promise<void> {
        this.subscribers--;

        if(this.subscribers == 0) {
            this.connection.off("subscribeToChanges");
        }
    }

    subscribe(listener: () => void) {
        this.listeners.push(listener);
    }

    unsubscribe(listener: () => void) {
        this.listeners = this.listeners.filter(l => l !== listener);
    }
}