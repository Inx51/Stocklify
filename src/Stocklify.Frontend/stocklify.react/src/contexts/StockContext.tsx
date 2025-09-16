import { createContext, useMemo, useContext, useEffect, useState } from "react";
import { StockService } from "../services/StockService";
import * as signalR from '@microsoft/signalr';

export const StockContext = createContext<StockService | null>(null);

export function StockProvider({ children }: { children: React.ReactNode }) {
    const [stockService, setStockService] = useState<StockService | null>(null);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:8085/hub/stockValueServiceHub")
            .withAutomaticReconnect()
            .build();

        const service = new StockService(connection);
        
        // Connect to the service
        service.connect()
            .then(() => {
                setStockService(service);
            })
            .catch(error => {
                console.error("Failed to connect to the stock service:", error);
            });

        // Cleanup: disconnect when component unmounts
        return () => {
            service.disconnect();
        };
    }, []);

    return (
        <StockContext.Provider value={stockService}>
            {children}
        </StockContext.Provider>
    );
};

export const useStockService = (): StockService | null => {
    return useContext(StockContext);
};