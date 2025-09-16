import { useEffect, useState } from 'react';
import ToolButton from '../ToolButton/ToolButton';
import './StockCard.css';
import { BriefcaseIcon, EyeIcon, ScaleIcon } from '@heroicons/react/24/outline'
import { BriefcaseIcon as BriefcaseIconSolid, EyeIcon as EyeIconSolid, ScaleIcon as ScaleIconSolid } from '@heroicons/react/24/solid'
import { useStockService } from '../../contexts/StockContext';
import { Stock } from '../../types/Stock';
import { IndexChart } from '../IndexChart/IndexChart';

function StockCard({ stock }: { stock: Stock }) {

    const startPrice = stock.value;
    const [price, setPrice] = useState<number>(startPrice);
    const [changePercentage, setChangePercentage] = useState<number>(calculateChangePercentage(price));
    const [priceHistory, setPriceHistory] = useState<number[]>([1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20]);

    const stockService = useStockService();

    function calculateChangePercentage(newPrice: number): number {
        return (newPrice / startPrice) -1;
    }

    useEffect(() => {
        stockService!.subscribe(stock.stockId, (value: number) => {
            setPrice(value);
            setPriceHistory(prev => [...prev, value]);
            setChangePercentage(calculateChangePercentage(value));
        });
    },[stockService]);

    return (
        <div>
            <div className="stock-card" tabIndex={0}>
                {/* <h2>{stockId}</h2> */}
                <div className="flex">
                    <div className="flex-1 text-gray-300 font-semibold">
                        XXXXXXXX
                    </div>
                    <div className="flex flex-1 text-white font-bold justify-end">
                        <div className={`stock-change-label ${changePercentage >= 0 ? 'stock-change-label-positive' : 'stock-change-label-negative'} font-thin text-xs`}>{changePercentage.toFixed(2)}%</div>
                    </div>
                </div>
                <div className="text-white font-bold">
                    ${price.toFixed(3)}
                </div>
                {/* <div className={`mt-5 fake-graph ${changePercentage >= 0 ? 'graph-positive' : 'graph-negative'}`}>
                    
                </div> */}
                <IndexChart values={priceHistory} />
                <div className="toolbar flex mt-1 gap-x-1">
                    <ToolButton Icon={<BriefcaseIcon className="size-5 text-white icon" />} ActiveIcon={<BriefcaseIconSolid className="size-5 text-white icon" />} Tooltip="Add stock to portfolio" />
                    <ToolButton Icon={<EyeIcon className="size-5 text-white icon" />} ActiveIcon={<EyeIconSolid className="size-5 text-white icon" />} Tooltip="Add stock to watch list" />
                    <ToolButton Icon={<ScaleIcon className="size-5 text-white icon" />} ActiveIcon={<ScaleIconSolid className="size-5 text-white icon" />} Tooltip="Compare stocks" />
                </div>
            </div>
        </div>
    );
}

export default StockCard;