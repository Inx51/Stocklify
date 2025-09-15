import { useEffect, useState } from 'react';
import StockCard from '../StockCard/StockCard';
import { useStockService } from '../../contexts/StockContext';
import { Stock } from '../../types/Stock';

function StockGrid() {

    const stockService = useStockService();

    const [stocks, setStocks] = useState<Stock[]>([]);

    useEffect(() => {
        const fetchStocks = async () => {
            if (stockService) {
                const fetchedStocks = await stockService.getStocks();
                setStocks(fetchedStocks);
            }
        }
        fetchStocks();
    }, [stockService]);

    return (
        <div className="grid lg:grid-cols-3 md:grid-cols-2 sm:grid-cols-1 gap-x-2 gap-y-2 md:gap-x-4 md:gap-y-4" style={{ width: "1050px" }}>
            {stocks.map((stock: Stock) => (
                <StockCard
                    key={stock.stockId}
                    stock={stock}
                />
            ))}
        </div>
    );
}

export default StockGrid;