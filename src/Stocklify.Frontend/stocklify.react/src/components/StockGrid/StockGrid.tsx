import { useEffect, useState } from 'react';
import StockCard from '../StockCard/StockCard';
import { useStockService } from '../../contexts/StockContext';
import { Stock } from '../../types/Stock';

function StockGrid() {

    const stockService = useStockService();
    const [stocks, setStocks] = useState({});

    useEffect(() => {
        if(stockService?.stocks !== undefined) {
            setStocks(stockService?.stocks)
            stockService?.startSubscribeToChanges();
        }

        const handleStockUpdate = () => {
            setStocks({ ...stockService!.stocks });
        };
        stockService?.subscribe(handleStockUpdate);

        return () => {
            stockService?.unsubscribe(handleStockUpdate);
            stockService?.stopSubscribeToChanges();
        }
    }, [stockService]);
    
    if(Object.keys(stocks).length === 0) {
        return <div>No stocks available</div>;
    }

    return (
        <div className="grid lg:grid-cols-3 md:grid-cols-2 sm:grid-cols-1 gap-x-2 gap-y-2 md:gap-x-4 md:gap-y-4" style={{ width: "1050px" }}>
            {Object.values(stocks).map((stock:any) => (
                <StockCard 
                    key={stock.stockId}
                    stockId={stock.stockId}
                />
            ))}
        </div>
    );
}

export default StockGrid;