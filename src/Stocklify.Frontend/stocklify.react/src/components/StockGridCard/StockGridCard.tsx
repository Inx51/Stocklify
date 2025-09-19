import Card from "../Card/Card"

function StockGridCard({children}: {children: React.ReactNode}) {
    return (
        <Card>
            {children}
        </Card>
    )
}

export default StockGridCard