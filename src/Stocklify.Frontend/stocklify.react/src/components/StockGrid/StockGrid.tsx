import { useEffect, useRef, useState } from "react";
import LayoutGrid from "../LayoutGrid/LayoutGrid";
import StockGridCard from "../StockGridCard/StockGridCard";

function StockGrid() {

    const numOfCards = 9;
    const intervalMs = 200;
    const animationFrameId = useRef<number>(0);
    const lastUpdateTime = useRef(performance.now())
    const [tick, setTick] = useState(0);

    useEffect(() => {
        //The update function that gets called at a fixed interval.
        const update = () => {
            setTick(tick => tick + 1);
        }

        //An efficient way to update at a fixed interval using requestAnimationFrame.
        const tickLoop = (now: number) => {
            if(now - lastUpdateTime.current >= intervalMs) {
                update();
                lastUpdateTime.current = now;
            }
            animationFrameId.current = requestAnimationFrame(tickLoop);
        }
        animationFrameId.current = requestAnimationFrame(tickLoop);
        return () => {
            cancelAnimationFrame(animationFrameId.current);
        }
    }, []);

    return (
        <LayoutGrid>
            {Array.from({ length: numOfCards }).map((_, i) => (
                <StockGridCard>
                    <div className="text-white">Hello {i+1} time{i > 0 ? "s" : ""}</div>
                    <div className="text-white">Tick: <span className="font-bold">{tick}</span></div>
                </StockGridCard>
            ))}
        </LayoutGrid>)
}

export default StockGrid;