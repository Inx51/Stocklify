import { useEffect, useState } from 'react';
import ToolButton from '../ToolButton/ToolButton';
import './StockCard.css';
import { BriefcaseIcon, EyeIcon, ScaleIcon } from '@heroicons/react/24/outline'
import { BriefcaseIcon as BriefcaseIconSolid, EyeIcon as EyeIconSolid, ScaleIcon as ScaleIconSolid } from '@heroicons/react/24/solid'
import { useStockService } from '../../contexts/StockContext';

function StockCard({ stockId }: { stockId: number }) {

    const stockService = useStockService();

    const stock = stockService!.stocks![`${stockId}`];

    const [symbol, setSymbol] = useState("XXX");
    const [price, setPrice] = useState(0);
    const [changePercentage, setChangePercentage] = useState(0);
    const [startPrice, setStartPrice] = useState<number>(() => stock.value);

    useEffect(() => {
        setStartPrice(stock.value);
    }, [stockId]);

    useEffect(() => {
        setPrice(stock.value);
        setChangePercentage((stock.value / startPrice) - 1);
    }, [stock.value, startPrice]);

    useEffect(() => {
        // Fetch symbol based on stockId, here we use a placeholder
        setSymbol(`STK${stockId}`);
    }, [stockId]);


    return (
        <div>
            <div className="stock-card" tabIndex={0}>
                <div className="flex">
                    <div className="flex-1 text-gray-300 font-semibold">
                        {symbol}
                    </div>
                    <div className="flex flex-1 text-white font-bold justify-end">
                        <div className={`stock-change-label ${changePercentage >= 0 ? 'stock-change-label-positive' : 'stock-change-label-negative'} font-thin text-xs`}>{changePercentage.toFixed(2)}%</div>
                    </div>
                </div>
                <div className="text-white font-bold">
                    ${price.toFixed(3)}
                </div>
                <div className={`mt-5 fake-graph ${changePercentage >= 0 ? 'graph-positive' : 'graph-negative'}`}>

                </div>
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