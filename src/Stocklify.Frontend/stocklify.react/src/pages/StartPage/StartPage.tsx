import { IndexChart } from "../../components/IndexChart/IndexChart";
import LayoutCentered from "../../components/LayoutCentered/LayoutCentered";
import SearchBar from "../../components/SearchBar/SearchBar";
import StockGrid from "../../components/StockGrid/StockGrid";
import { StockProvider } from "../../contexts/StockContext";

function StartPage() {
    return (
        <div>
            <LayoutCentered>
                <SearchBar />
            </LayoutCentered>
            <LayoutCentered>
                <StockProvider>
                    <StockGrid />
                </StockProvider>
            </LayoutCentered>
        </div>
    );
}

export default StartPage;